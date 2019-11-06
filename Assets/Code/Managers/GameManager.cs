namespace Managers {
    using Structures;
    using UI.Interfaces;
    using Controllers.Interfaces;
    using Controllers;
    using System;
    using System.Threading.Tasks;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public PoolOptions PoolOptions;

        [SerializeField] private JoystickController joystickController;
        [SerializeField] private Player             player;
        [SerializeField] private float              cameraYAxisOnMobilePlatform = -25f;
        [SerializeField] private float              respawnTime;
        [SerializeField] private Transform          respawnPoint;

        private InputController playerInputController;
        private Joystick        joystick;

        private PoolManager              poolManager;
        private CommandController        playerCommandController;
        private CommandsForBotController commandsForBotController;
        private AnimatorManager          animatorManager;

        private CameraController   cameraController;
        private MovementController movementController;
        private WeaponController   weaponController;

        private float mouseX;
        private float mouseY;
        private bool  isJoystickOn;
        private bool  isRespawning;

        private void Awake() {
            this.player.OnDeath += () => this.isRespawning = true;
        }

        private void Respawn() {
            this.player.transform.position = Vector3.Lerp(
                this.player.transform.position, this.respawnPoint.position, Time.deltaTime);

            if (Vector3.Distance(this.player.transform.position, this.respawnPoint.position) <= 0.1f) {
                this.isRespawning = false;
                this.player.gameObject.SetActive(true);
            }
        }

        private void Start() => this.Initialize();

        private async void Initialize() {
            await this.GetSingletons();

            if (!Application.isMobilePlatform) {
                this.playerInputController = this.joystickController;
                this.joystick              = this.playerInputController as Joystick;
                this.isJoystickOn          = true;

                this.weaponController.OnWeaponChanged += (rate, sprite, isThrowable) => this.joystick.RearrangeAttackUI(sprite, isThrowable);
            }
            else {
                this.playerInputController = this.gameObject.AddComponent<KeyboardController>();
                this.isJoystickOn          = false;
            }

            this.joystickController.gameObject.SetActive(this.isJoystickOn);

            this.playerCommandController.Initialize(this.playerInputController);

            this.animatorManager.OnWeaponEquip       += this.weaponController.OnWeaponEquip;
            this.weaponController.OnWeaponRearranged += this.animatorManager.SetupWeaponCondition;
            this.weaponController.SetWeaponEquipped  += this.animatorManager.SetWeaponEquipping;
            this.weaponController.OnWeaponChanged    += (rate, sprite, isThrowable) => this.playerCommandController.SerialRate = rate;

            this.weaponController.OnWeaponChanged += (rate, sprite, isThrowable) => this.movementController.SetAimIK(!isThrowable);

            this.playerCommandController.OnFireOnce       += this.weaponController.OnFire;
            this.playerCommandController.OnReload         += this.weaponController.Reload;
            this.playerCommandController.OnChangingWeapon += this.weaponController.ChangeWeapon;

            this.weaponController.Initialize();

            this.poolManager.Initialize(this.PoolOptions.Pools);
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

        private void Update() {
            this.mouseX = this.playerCommandController.RotateX;
            this.mouseY = this.playerCommandController.RotateY;

            this.movementController.HorizontalMoving = this.playerCommandController.HorizontalMoving;
            this.movementController.ForwardMoving    = this.playerCommandController.ForwardMoving;
            this.movementController.IsJumpPressed    = this.playerCommandController.IsJumping;

            this.movementController.RotatePlayer(this.mouseX);

            this.UpdateAnimatorState();

            if (this.isRespawning) {
                this.Respawn();
            }
        }

        private void FixedUpdate() {
            this.movementController.Move(
                this.playerCommandController.ForwardMoving,
                this.playerCommandController.HorizontalMoving);
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