namespace Managers {
    using UI.Abstract;
    using Abilities;
    using Controllers;
    using UnityEngine;

    public class PlayerManager : CharacterManager {
        [SerializeField] private float cameraYAxisOnMobilePlatform = -30f;

        private PlayerCommandController playerCommandController;
        private Joystick                joystick;
        private CameraController        cameraController;

        private bool isJoystickOn;

        public float SerialRate {
            set => this.playerCommandController.SerialRate = value;
        }

        protected new void Start() {
            this.GetSingletones();
            base.Start();
            this.EstablishSubscriptions();
        }

        private void Update() {
            var movingSpeed = this.statManager.CalculateValue(StatType.MovingSpeed, this.movingSpeed);

            var forwardMoving    = this.playerCommandController.ForwardMoving * movingSpeed * Time.fixedDeltaTime;
            var horizontalMoving = 0f;

            if (!this.isJoystickOn) {
                horizontalMoving = this.playerCommandController.HorizontalMoving * movingSpeed * Time.fixedDeltaTime;
            }

            this.movementController.ForwardMoving    = forwardMoving;
            this.movementController.HorizontalMoving = horizontalMoving;

            this.movementController.IsJumpPressed = this.playerCommandController.IsJumping;

            this.movementController.RotateTheCharacter();

            this.weaponController.BallisticValue = this.playerCommandController.BallisticValue;

            this.movementController.RotationX = this.playerCommandController.RotationX;

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
            this.weaponController.OnWeaponChanged    += (rate, sprite, isThrowable) => this.movementController.SetAimIK(!isThrowable);

            this.playerCommandController.OnFireOnce       += this.weaponController.OnFire;
            this.playerCommandController.OnReload         += this.weaponController.Reload;
            this.playerCommandController.OnChangingWeapon += this.weaponController.ChangeWeapon;
        }

        private void FixedUpdate() => this.movementController.Move();

        private void LateUpdate() {
            this.cameraController.RotateTargetHorizontally(this.playerCommandController.RotationX);

            if (this.isJoystickOn) {
                this.cameraController.RotateCamera(this.playerCommandController.RotationX, this.cameraYAxisOnMobilePlatform);
            }
            else {
                this.cameraController.RotateCamera(this.playerCommandController.RotationX, this.playerCommandController.RotationY);
            }
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving = this.playerCommandController.ForwardMoving;

            if (!this.isJoystickOn) {
                this.animatorManager.HorizontalMoving = this.playerCommandController.HorizontalMoving;
            }

            this.animatorManager.IsSneak        = this.playerCommandController.IsSneak;
            this.animatorManager.IsJumping      = this.movementController.IsJumping;
            this.animatorManager.IsFalling      = this.movementController.IsFalling();
            this.animatorManager.IsGrounded     = this.movementController.IsGrounded();
            this.animatorManager.IsNearToGround = this.movementController.IsNearToGround();
        }
    }
}