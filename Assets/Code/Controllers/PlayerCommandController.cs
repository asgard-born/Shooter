namespace Controllers {
    using Abstract;
    using System;
    using UnityEngine;

    public class PlayerCommandController : MonoBehaviour, ICommandController {
        public static PlayerCommandController Instance;

        public JoystickController JoystickController;

        public float ForwardMoving       => this.forwardMoving;
        public float HorizontalMoving    => this.horizontalMoving;
        public float RotationX           => this.rotationX;
        public float RotationY           => this.rotationY;
        public float BallisticValue      => this.JoystickController.GetBallisticValue;
        public float AttackRotatingValue => this.JoystickController.GetAttackJoystickValue;
        public bool  IsSneak             => this.isSneak;
        public bool  IsJumping           => this.isJumping;

        private float rotationX;
        private float rotationY;

        private float forwardMoving;
        private float horizontalMoving;
        private bool  isSneak;
        private bool  isJumping;

        public event Action OnFireOnce;
        public event Action OnReload;
        public event Action OnChangingWeapon;

        private IInputController inputController;

        private bool canInputFire = true;
        private bool isJoystick;

        public float SerialRate {
            set => this.inputController.SerialRate = value;
        }

        public IInputController Initialize() {
            if (Application.isMobilePlatform) {
                this.inputController = this.JoystickController;
                this.isJoystick      = true;
            }
            else {
                this.inputController = this.gameObject.AddComponent<KeyboardController>();
                this.isJoystick      = false;
            }

            this.JoystickController.gameObject.SetActive(this.isJoystick);
            this.inputController.OnFireOnce       += () => this.OnFireOnce?.Invoke();
            this.inputController.OnReload         += () => this.OnReload?.Invoke();
            this.inputController.OnChangingWeapon += () => this.OnChangingWeapon?.Invoke();

            return this.inputController;
        }

        private void Awake() => Instance = this;

        private void Update() {
            if (this.isJoystick) {
                this.rotationX += this.AttackRotatingValue == 0 ? this.inputController.GetAxisX : this.AttackRotatingValue;
            }
            else {
                this.rotationX += this.inputController.GetAxisX;
            }

            this.rotationY -= this.inputController.GetAxisY;

            if (this.isJoystick) {
                this.forwardMoving    = this.inputController.ForwardMoving > 0 ? 1 : this.inputController.ForwardMoving < 0 ? -1 : 0;
                this.horizontalMoving = this.inputController.HorizontalMoving > 0 ? 1 : 0;
            }
            else {
                this.forwardMoving    = this.inputController.ForwardMoving;
                this.horizontalMoving = this.inputController.HorizontalMoving;
            }

            this.isSneak = this.inputController.IsSneak;

            this.isJumping = this.inputController.IsJumping;
        }
    }
}