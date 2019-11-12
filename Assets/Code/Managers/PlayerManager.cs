using System;
using System.Threading.Tasks;

namespace Managers {
    using Controllers;
    using Structures;
    using UI.Interfaces;
    using UnityEngine;

    public class PlayerManager : Character {
        private CameraController        cameraController;
        private AnimatorManager         animatorManager;
        private MovementController      movementController;
        private PlayerCommandController playerCommandController;
        private float                   mouseX;
        private float                   mouseY;

        public  WeaponController WeaponController => this.weaponController;
        private WeaponController weaponController;

        [SerializeField] private float cameraYAxisOnMobilePlatform = -30f;

        private Joystick joystick;
        private bool     isJoystickOn;

        private async void Start() {
            this.GetLogicEssences();
            this.GetSingletones();
            this.EstablishSubscriptions();
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.Initialize();
            
        }

        private void Initialize() {
            this.weaponController.Initialize();
        }

        private void Update() {
            this.movementController.HorizontalMoving = this.playerCommandController.HorizontalMoving;
            this.movementController.ForwardMoving    = this.playerCommandController.ForwardMoving;
            this.movementController.IsJumpPressed    = this.playerCommandController.IsJumping;

            this.movementController.RotatePlayer(this.mouseX);

            this.weaponController.BallisticValue = this.playerCommandController.BallisticValue;

            this.mouseX = this.playerCommandController.RotateX;
            this.mouseY = this.playerCommandController.RotateY;

            this.UpdateAnimatorState();
        }

        private void GetSingletones() {
            this.cameraController        = CameraController.Instance;
            this.playerCommandController = PlayerCommandController.Instance;
        }

        private void GetLogicEssences() {
            this.animatorManager    = this.GetComponent<AnimatorManager>();
            this.movementController = this.GetComponent<MovementController>();
            this.weaponController   = this.GetComponent<WeaponController>();
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

        private void FixedUpdate() {
            var horizontalMoving = 0f;

            if (!this.isJoystickOn) {
                horizontalMoving = this.playerCommandController.HorizontalMoving;
            }

            this.movementController.Move(
                this.playerCommandController.ForwardMoving,
                horizontalMoving);
        }

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
            this.animatorManager.IsJumping      = this.movementController.IsJumping;
            this.animatorManager.IsFalling      = this.movementController.IsFalling();
            this.animatorManager.IsGrounded     = this.movementController.IsGrounded();
            this.animatorManager.IsNearToGround = this.movementController.IsNearToGround();
        }
    }
}