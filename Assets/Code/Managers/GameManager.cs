namespace Managers {
    using Controllers.Interfaces;
    using Controllers;
    using System;
    using System.Threading.Tasks;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public PoolOptions PoolOptions;

        private PoolManager              poolManager;
        private CommandController        playerCommandController;
        private InputController          playerInputController;
        private InputController          playerInputControllerAlt;
        private CommandsForBotController commandsForBotController;
        private AnimatorManager          animatorManager;

        private CameraController   cameraController;
        private MovementController movementController;
        private WeaponController   weaponController;

        private float mouseX;
        private float mouseY;

        private void Start() {
            this.Initialize();
        }

        private async Task GetSingletons() {
            this.poolManager             = PoolManager.Instance;
            this.playerCommandController = PlayerCommandController.Instance;
            this.animatorManager         = AnimatorManager.Instance;

            this.cameraController   = CameraController.Instance;
            this.movementController = MovementController.Instance;
            this.weaponController   = WeaponController.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }

            while (this.playerCommandController == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.playerCommandController = PlayerCommandController.Instance;
            }

            while (this.animatorManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.animatorManager = AnimatorManager.Instance;
            }

            while (this.cameraController == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.cameraController = CameraController.Instance;
            }

            while (this.movementController == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.movementController = MovementController.Instance;
            }

            while (this.weaponController == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.weaponController = WeaponController.Instance;
            }
        }

        private async void Initialize() {
            await this.GetSingletons();
            this.playerInputController    = this.gameObject.AddComponent<KeyboardController>();
            this.playerInputControllerAlt = this.gameObject.AddComponent<JoystickController>();

            this.playerCommandController.Initialize(this.playerInputController);

            this.animatorManager.OnWeaponEquip          += this.weaponController.OnWeaponEquip;
            this.weaponController.OnWeaponRearranged    += this.animatorManager.SetupWeaponCondition;
            this.weaponController.SetWeaponEquipped     += this.animatorManager.SetWeaponEquipping;
            this.weaponController.OnWeaponChanged       += rate => this.playerCommandController.SerialRate = rate;
            this.weaponController.OnThrowingWeaponEquip += isThrowable => this.movementController.SetAimIK(!isThrowable);

            this.playerCommandController.OnFireOnce += this.weaponController.OnFire;
            this.playerCommandController.OnReload   += this.weaponController.Reload;

            this.weaponController.Initialize();

            this.poolManager.Initialize(this.PoolOptions.Pools);
        }

        private void Update() {
            this.mouseX = this.playerCommandController.RotateX;
            this.mouseY = this.playerCommandController.RotateY;

            this.movementController.HorizontalMoving = this.playerCommandController.HorizontalMoving;
            this.movementController.ForwardMoving    = this.playerCommandController.ForwardMoving;
            this.movementController.IsJumpPressed    = this.playerCommandController.IsJumping;


            this.movementController.RotatePlayer(this.mouseX);

            this.UpdateAnimatorState();

            Cursor.visible = false;
        }

        private void FixedUpdate() {
            this.movementController.Move(
                this.playerCommandController.ForwardMoving * Time.fixedDeltaTime,
                this.playerCommandController.HorizontalMoving * Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            this.cameraController.RotateTargetHorizontally(this.mouseX);
            this.cameraController.RotateCamera(this.mouseX, this.mouseY);
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving    = this.playerCommandController.ForwardMoving;
            this.animatorManager.HorizontalMoving = this.playerCommandController.HorizontalMoving;
            this.animatorManager.IsSneak          = this.playerCommandController.IsSneak;
            this.animatorManager.IsJumping        = this.movementController.IsJumping;
            this.animatorManager.IsFalling        = this.movementController.IsFalling();
            this.animatorManager.IsGrounded       = this.movementController.IsGrounded();
            this.animatorManager.IsNearToGround   = this.movementController.IsNearToGround();
        }
    }
}