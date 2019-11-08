using System;
using Controllers.Interfaces;
using UnityEngine;

namespace Controllers {
    public class PlayerCommandController : MonoBehaviour, CommandController {
        public static PlayerCommandController Instance;

        private                  InputController    playerInputController;
        [SerializeField] private JoystickController joystickController;

        public float ForwardMoving    => this.forwardMoving;
        public float HorizontalMoving => this.horizontalMoving;
        public float RotateX          => this.rotateX;
        public float RotateY          => this.rotateY;
        public bool  IsSneak          => this.isSneak;
        public bool  IsJumping        => this.isJumping;

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

        public float SerialRate {
            set => this.inputController.SerialRate = value;
        }

        public InputController Initialize() {
            bool isJoystick;

            if (!Application.isMobilePlatform) {
                this.inputController = this.joystickController;
                isJoystick           = true;
            }
            else {
                this.playerInputController = this.gameObject.AddComponent<KeyboardController>();
                isJoystick                 = false;
            }

            this.joystickController.gameObject.SetActive(isJoystick);
            this.inputController.OnFireOnce       += () => this.OnFireOnce?.Invoke();
            this.inputController.OnReload         += () => this.OnReload?.Invoke();
            this.inputController.OnChangingWeapon += () => this.OnChangingWeapon?.Invoke();

            return this.inputController;
        }

        private void Awake() => Instance = this;

        private void Update() {
            this.rotateX += this.inputController.GetAxisX;
            this.rotateY -= this.inputController.GetAxisY;

            this.forwardMoving    = this.inputController.ForwardMoving;
            this.horizontalMoving = this.inputController.HorizontalMoving;
            this.isSneak          = this.inputController.IsSneak;

            this.isJumping = this.inputController.IsJumping;
        }
    }
}