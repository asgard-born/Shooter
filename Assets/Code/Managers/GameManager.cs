using Weapons.ConcreteWeapons;
using Weapons.Options;
using Weapons.WeaponTypes;

namespace Managers {
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public PoolOptions PoolOptions;

        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform     respawnPoint;

        // Singletons
        private PoolManager    poolManager;
        private EnemiesManager enemiesManager;

        private bool isRespawning;

        private void Awake() 
            => this.playerManager.OnDeath += () => this.isRespawning = true;

        private async void Start() {
            this.GetSingletones();
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.ParseJSONData();
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.Initialize();
        }

        private void Initialize() {
            this.poolManager.Initialize(this.PoolOptions.Pools);
            this.InitializeTheEnemies();
        }

        private void ParseJSONData() {
            var asset = Resources.Load<TextAsset>("GameOptions/weapon_options");

            if (asset != null) {
                var weaponOptionsContainer = JsonUtility.FromJson<WeaponOptionsList>(asset.text);

                var weaponOptions = weaponOptionsContainer.weaponOptions;

                foreach (var weapon in this.playerManager.WeaponController.Weapons) {
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
                var additionalSerialRate = 3.8f;
                var enemyDamage          = 5;

                foreach (var enemy in this.enemiesManager.Enemies) {
                    enemy.EnemyCommandController.SerialRate = (weaponOptions.First(option => option.id == 1).serialRate + additionalSerialRate);

                    foreach (var weapon in enemy.WeaponController.Weapons) {
                        var findedOption = weaponOptions.First(option => option.id == weapon.Id);

                        weapon.WeaponName = findedOption.weaponName;
                        weapon.SerialRate = findedOption.serialRate;
                        weapon.Damage     = enemyDamage;
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
            this.playerManager.transform.position = Vector3.Lerp(
                this.playerManager.transform.position, this.respawnPoint.position, Time.deltaTime);

            if (Vector3.Distance(this.playerManager.transform.position, this.respawnPoint.position) <= 0.5f) {
                this.isRespawning                     = false;
                this.playerManager.transform.position = this.respawnPoint.position;
                this.playerManager.gameObject.SetActive(true);
                this.playerManager.Respawn();
            }
        }

        private void GetSingletones() {
            this.poolManager    = PoolManager.Instance;
            this.enemiesManager = EnemiesManager.Instance;
        }

        private void InitializeTheEnemies()
            => this.enemiesManager.Initialize(this.playerManager.transform);

        private void Update() {
            if (this.isRespawning) {
                this.Respawn();
            }
        }
    }
}