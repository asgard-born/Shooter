namespace Managers {
    using Controllers;
    using System;
    using System.Threading.Tasks;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public PoolOptions PoolOptions;

        private PoolManager     poolManager;
        private InputController inputController;
        private AnimatorManager animatorManager;

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

            this.animatorManager.OnWeaponEquip      += this.weaponController.OnWeaponEquip;
            this.weaponController.OnWeaponChanged   += this.animatorManager.SetupWeaponCondition;
            this.weaponController.SetWeaponEquipped += this.animatorManager.SetWeaponEquipping;
            this.inputController.OnMouseButtonDown  += this.weaponController.Fire;

            this.weaponController.SetupTheWeapon(true);

            this.poolManager.Initialize(this.PoolOptions.Pools);
        }

        private async Task GetSingletons() {
            this.poolManager     = PoolManager.Instance;
            this.inputController = InputController.Instance;
            this.animatorManager = AnimatorManager.Instance;

            this.cameraController   = CameraController.Instance;
            this.movementController = MovementController.Instance;
            this.weaponController   = WeaponController.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }

            while (this.inputController == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.inputController = InputController.Instance;
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
            this.mouseX = this.inputController.MouseX;
            this.mouseY = this.inputController.MouseY;

            this.movementController.HorizontalMoving = this.inputController.HorizontalMoving;
            this.movementController.ForwardMoving    = this.inputController.ForwardMoving;
            this.movementController.IsJumpPressed    = this.inputController.IsJumpPressed;

            if (this.inputController.RunValue == 1 || !this.weaponController.IsAiming) {
                this.weaponController.ChangeAimPositionWeight(0);
            }
            else {
                this.weaponController.ChangeAimPositionWeight(1);
            }


            this.movementController.RotatePlayer(this.mouseX);

            this.UpdateAnimatorState();

            Cursor.visible = false;
        }

        private void FixedUpdate() {
            this.movementController.Move(
                this.inputController.ForwardMoving    * Time.fixedDeltaTime,
                this.inputController.HorizontalMoving * Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            this.cameraController.RotateTargetHorizontally(this.mouseX);
            this.cameraController.RotateCamera(this.mouseX, this.mouseY);
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving    = this.inputController.ForwardMoving;
            this.animatorManager.HorizontalMoving = this.inputController.HorizontalMoving;
            this.animatorManager.IsSneak          = this.inputController.IsSneak;
            this.animatorManager.IsJumping        = this.movementController.IsJumping;
            this.animatorManager.IsFalling        = this.movementController.IsFalling();
            this.animatorManager.IsGrounded       = this.movementController.IsGrounded();
            this.animatorManager.IsNearToGround   = this.movementController.IsNearToGround();
        }
    }
}