namespace Managers {
    using Controllers;
    using System;
    using System.Threading.Tasks;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public PoolOptions PoolOptions;

        private PoolManager              poolManager;
        private PlayerInputController    playerInputController;
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

        private async void Initialize() {
            await this.GetSingletons();

            this.animatorManager.OnWeaponEquip       += this.weaponController.OnWeaponEquip;
            this.weaponController.OnWeaponRearranged += this.animatorManager.SetupWeaponCondition;
            this.weaponController.SetWeaponEquipped  += this.animatorManager.SetWeaponEquipping;
            this.weaponController.OnWeaponChanged    += rate => this.playerInputController.SerialRate = rate;
            this.playerInputController.OnFireOnce    += this.weaponController.OnFire;
            this.playerInputController.OnReload      += this.weaponController.Reload;

            this.weaponController.Initialize();

            this.poolManager.Initialize(this.PoolOptions.Pools);
        }

        private async Task GetSingletons() {
            this.poolManager           = PoolManager.Instance;
            this.playerInputController = PlayerInputController.Instance;
            this.animatorManager       = AnimatorManager.Instance;

            this.cameraController   = CameraController.Instance;
            this.movementController = MovementController.Instance;
            this.weaponController   = WeaponController.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }

            while (this.playerInputController == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.playerInputController = PlayerInputController.Instance;
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

        private void Update() {
            this.mouseX = this.playerInputController.RotateX;
            this.mouseY = this.playerInputController.RotateY;

            this.movementController.HorizontalMoving = this.playerInputController.HorizontalMoving;
            this.movementController.ForwardMoving    = this.playerInputController.ForwardMoving;
            this.movementController.IsJumpPressed    = this.playerInputController.IsJumping;


            this.movementController.RotatePlayer(this.mouseX);

            this.UpdateAnimatorState();

            Cursor.visible = false;
        }

        private void FixedUpdate() {
            this.movementController.Move(
                this.playerInputController.ForwardMoving    * Time.fixedDeltaTime,
                this.playerInputController.HorizontalMoving * Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            this.cameraController.RotateTargetHorizontally(this.mouseX);
            this.cameraController.RotateCamera(this.mouseX, this.mouseY);
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving    = this.playerInputController.ForwardMoving;
            this.animatorManager.HorizontalMoving = this.playerInputController.HorizontalMoving;
            this.animatorManager.IsSneak          = this.playerInputController.IsSneak;
            this.animatorManager.IsJumping        = this.movementController.IsJumping;
            this.animatorManager.IsFalling        = this.movementController.IsFalling();
            this.animatorManager.IsGrounded       = this.movementController.IsGrounded();
            this.animatorManager.IsNearToGround   = this.movementController.IsNearToGround();
        }
    }
}