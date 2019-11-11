namespace Managers {
    using Structures.Weapons.WeaponTypes;
    using System.Linq;
    using Structures.Weapons.ConcreteWeapons;
    using Structures.Weapons.Options;
    using Structures;
    using UI.Interfaces;
    using Controllers;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public PoolOptions PoolOptions;

        [SerializeField] private Player    player;
        [SerializeField] private float     cameraYAxisOnMobilePlatform = -25f;
        [SerializeField] private Transform respawnPoint;

        private Joystick joystick;

        // Singletons
        private PoolManager              poolManager;
        private AnimatorManager          animatorManager;
        private PlayerCommandController  playerCommandController;
        private CommandsForBotController commandsForBotController;
        private EnemiesManager           enemiesManager;

        private CameraController   cameraController;
        private MovementController movementController;
        private WeaponController   weaponController;

        private float mouseX;
        private float mouseY;
        private bool  isJoystickOn;
        private bool  isRespawning;

        private void Awake()
            => this.player.OnDeath += () => this.isRespawning = true;

        private void Start() {
            this.Initialize();

            var asset = Resources.Load<TextAsset>("GameOptions/weapon_options");

            if (asset != null) {
                var weaponOptionsContainer = JsonUtility.FromJson<WeaponOptionsList>(asset.text);

                var weaponOptions = weaponOptionsContainer.weaponOptions;


                foreach (var weapon in this.weaponController.Weapons) {
                    var findedOption = weaponOptions.First(option => option.id == weapon.Id);

                    weapon.WeaponName = findedOption.weaponName;
                    weapon.SerialRate = findedOption.serialRate;
                    weapon.Damage     = findedOption.damage;
                    weapon.Range      = findedOption.range;

                    if (weapon is Grenade grenade) {
                        grenade.BlowingTime = findedOption.blowingTime;

                        continue;
                    }

                    if (weapon is FiringWeapon firingWeapon) {
                        firingWeapon.AttackSpeed      = findedOption.attackSpeed;
                        firingWeapon.MagazineCapacity = findedOption.magazineCapacity;
                        firingWeapon.ReloadRate       = findedOption.reloadRate;
                    }
                }


                // for not to loosing after 2 seconds :)
                var additionalSerialRate = 1.8f;
                foreach (var enemy in this.enemiesManager.Enemies) {
                    enemy.EnemyCommandController.SerialRate = (weaponOptions.First(option => option.id == 1).serialRate + additionalSerialRate);

                    foreach (var weapon in enemy.WeaponController.Weapons) {
                        var findedOption = weaponOptions.First(option => option.id == weapon.Id);

                        weapon.WeaponName = findedOption.weaponName;
                        weapon.SerialRate = findedOption.serialRate;
                        weapon.Damage     = findedOption.damage;
                        weapon.Range      = findedOption.range;

                        if (weapon is Grenade grenade) {
                            grenade.BlowingTime = findedOption.blowingTime;

                            continue;
                        }

                        if (weapon is FiringWeapon firingWeapon) {
                            firingWeapon.AttackSpeed      = findedOption.attackSpeed;
                            firingWeapon.MagazineCapacity = findedOption.magazineCapacity;
                            firingWeapon.ReloadRate       = findedOption.reloadRate;
                        }
                    }
                }
            }
        }

        private void Respawn() {
            this.player.transform.position = Vector3.Lerp(
                this.player.transform.position, this.respawnPoint.position, Time.deltaTime);

            if (Vector3.Distance(this.player.transform.position, this.respawnPoint.position) <= 0.5f) {
                this.isRespawning              = false;
                this.player.transform.position = this.respawnPoint.position;
                this.player.gameObject.SetActive(true);
                this.player.Respawn();
            }
        }

        private void Initialize() {
            this.GetSingletones();
            this.GetLogicEssences();

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

            this.weaponController.Initialize();
            this.poolManager.Initialize(this.PoolOptions.Pools);

            this.InitializeTheEnemies();
        }

        private void GetLogicEssences() {
            this.animatorManager    = this.player.GetComponent<AnimatorManager>();
            this.movementController = this.player.GetComponent<MovementController>();
            this.weaponController   = this.player.GetComponent<WeaponController>();
        }

        private void GetSingletones() {
            this.poolManager             = PoolManager.Instance;
            this.playerCommandController = PlayerCommandController.Instance;
            this.enemiesManager          = EnemiesManager.Instance;
            this.cameraController        = CameraController.Instance;
        }

        private void InitializeTheEnemies() {
            this.enemiesManager.Initialize(this.player.transform);
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
                0);
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