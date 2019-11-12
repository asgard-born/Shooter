namespace Controllers {
    using System;
    using Interfaces;
    using UnityEngine;

    public class PlayerCommandController : MonoBehaviour, CommandController {
        public static PlayerCommandController Instance;

        [SerializeField] private JoystickController joystickController;

        public float ForwardMoving       => this.forwardMoving;
        public float HorizontalMoving    => this.horizontalMoving;
        public float RotateX             => this.rotateX;
        public float RotateY             => this.rotateY;
        public float BallisticValue      => this.joystickController.GetBallisticValue;
        public float AttackRotatingValue => this.joystickController.GetAttackJoystickValue;
        public bool  IsSneak             => this.isSneak;
        public bool  IsJumping           => this.isJumping;

        private float rotateX;
        private float rotateY;

        private float forwardMoving;
        private float horizontalMoving;
        private bool  isSneak;
        private bool  isJumping;

        public event Action OnFireOnce;
        public event Action OnReload;
        public event Action OnChangingWeapon;

        private InputController inputController;

        private bool canInputFire = true;
        private bool isJoystick;

        public float SerialRate {
            set => this.inputController.SerialRate = value;
        }

        public InputController Initialize() {
            if (!Application.isMobilePlatform) {
                this.inputController = this.joystickController;
                this.isJoystick      = true;
            }
            else {
                this.inputController = this.gameObject.AddComponent<KeyboardController>();
                this.isJoystick      = false;
            }

            this.joystickController.gameObject.SetActive(this.isJoystick);
            this.inputController.OnFireOnce       += () => this.OnFireOnce?.Invoke();
            this.inputController.OnReload         += () => this.OnReload?.Invoke();
            this.inputController.OnChangingWeapon += () => this.OnChangingWeapon?.Invoke();

            return this.inputController;
        }

        private void Awake() => Instance = this;

        private void Update() {
            this.rotateX += this.inputController.GetAxisX;
            this.rotateY -= this.inputController.GetAxisY;

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