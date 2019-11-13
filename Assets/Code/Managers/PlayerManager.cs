namespace Managers {
    using Abilities;
    using Controllers;
    using UI.Interfaces;
    using UnityEngine;

    public class PlayerManager : CharacterManager {
        [SerializeField] private float cameraYAxisOnMobilePlatform = -30f;
        [SerializeField] private float MovingSpeed                 = 5f;

        private PlayerCommandController playerCommandController;
        private Joystick                joystick;
        private CameraController        cameraController;

        private float mouseX;
        private float mouseY;
        private bool  isJoystickOn;

        public float SerialRate {
            set => this.playerCommandController.SerialRate = value;
        }

        protected new void Start() {
            this.GetSingletones();
            base.Start();
            this.EstablishSubscriptions();
        }

        private void Update() {
            var movingSpeed = this.statManager.CalculateValue(StatType.MovingSpeed, this.MovingSpeed);

            var forwardMoving    = this.playerCommandController.ForwardMoving * movingSpeed * Time.fixedDeltaTime;
            var horizontalMoving = 0f;

            if (!this.isJoystickOn) {
                horizontalMoving = this.playerCommandController.HorizontalMoving * movingSpeed * Time.fixedDeltaTime;
            }

            this.MovementController.ForwardMoving    = forwardMoving;
            this.MovementController.HorizontalMoving = horizontalMoving;

            this.MovementController.IsJumpPressed = this.playerCommandController.IsJumping;

            this.MovementController.RotatePlayer(this.mouseX);

            this.weaponController.BallisticValue = this.playerCommandController.BallisticValue;

            this.mouseX = this.playerCommandController.RotateX;
            this.mouseY = this.playerCommandController.RotateY;

            this.UpdateAnimatorState();
        }

        private void GetSingletones() {
            this.cameraController        = CameraController.Instance;
            this.playerCommandController = PlayerCommandController.Instance;
        }

        private void EstablishSubscriptions() {
            this.joystick = this.playerCommandController.Initialize() as Joystick;

            if (this.joystick != null) {
                this.weaponController.OnWeaponChanged += (rate, sprite, isThrowable) => this.joystick.RearrangeAttackUI(sprite, isThrowable);
                this.isJoystickOn                     =  true;
            }
            else {
                this.isJoystickOn = false;
            }

            this.animatorManager.OnWeaponEquip += this.weaponController.OnWeaponEquip;

            this.weaponController.OnWeaponRearranged += this.animatorManager.SetupWeaponCondition;
            this.weaponController.SetWeaponEquipped  += this.animatorManager.SetWeaponEquipping;
            this.weaponController.OnWeaponChanged    += (rate, sprite, isThrowable) => this.playerCommandController.SerialRate = rate;
            this.weaponController.OnWeaponChanged    += (rate, sprite, isThrowable) => this.MovementController.SetAimIK(!isThrowable);

            this.playerCommandController.OnFireOnce       += this.weaponController.OnFire;
            this.playerCommandController.OnReload         += this.weaponController.Reload;
            this.playerCommandController.OnChangingWeapon += this.weaponController.ChangeWeapon;
        }

        private void FixedUpdate() => this.MovementController.Move();

        private void LateUpdate() {
            this.cameraController.RotateTargetHorizontally(this.mouseX);

            if (this.isJoystickOn) {
                this.cameraController.RotateCamera(this.mouseX, this.cameraYAxisOnMobilePlatform);
            }
            else {
                this.cameraController.RotateCamera(this.mouseX, this.mouseY);
            }
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving = this.playerCommandController.ForwardMoving;

            if (!this.isJoystickOn) {
                this.animatorManager.HorizontalMoving = this.playerCommandController.HorizontalMoving;
            }

            this.animatorManager.IsSneak        = this.playerCommandController.IsSneak;
            this.animatorManager.IsJumping      = this.MovementController.IsJumping;
            this.animatorManager.IsFalling      = this.MovementController.IsFalling();
            this.animatorManager.IsGrounded     = this.MovementController.IsGrounded();
            this.animatorManager.IsNearToGround = this.MovementController.IsNearToGround();
        }
    }
}