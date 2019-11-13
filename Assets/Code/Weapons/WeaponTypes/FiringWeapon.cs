namespace Weapons.WeaponTypes {
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using Abstract;
    using Random = UnityEngine.Random;

    public abstract class FiringWeapon : Weapon, IReloadable {
        public    int   MagazineCapacity;
        public    int   AttackSpeed;
        public    float ReloadRate;
        protected int   Ammo;

        [Range(.02f, .2f)] public float Splash = .1f;

        [SerializeField] protected Transform aim;

        protected bool isReloading;

        public bool IsReloading => this.isReloading;

        public async Task Reload() {
            this.isReloading = true;
            this.Ammo        = this.MagazineCapacity;
            await Task.Delay(TimeSpan.FromSeconds(this.ReloadRate));
            this.isReloading = false;

            Debug.Log("finish reloading");
        }

        protected Quaternion CalculateFiringRotationWithSplash() {
            var verticalRandomDirectionPart   = (this.aim.forward + this.aim.up * Random.Range(-this.Splash, this.Splash));
            var horizontalRandomDirectionPart = (this.aim.forward + this.aim.right * Random.Range(-this.Splash, this.Splash));

            var direction      = verticalRandomDirectionPart + horizontalRandomDirectionPart;
            var firingRotation = Quaternion.LookRotation(direction);

            return firingRotation;
        }

        protected new void Awake() {
            base.Awake();
            this.Ammo = this.MagazineCapacity;
        }

        private void Update() {
            if (this.Ammo == 0) {
                this.Reload();
            }
        }

        protected void InitializeTheBullet(Bullet bullet, int id_attacker, LayerMask layerMask) {
            if (id_attacker == 16) {
                Debug.Log(this.CalculatedDamage);
            }

            bullet.Initialize(
                bullet.transform.position,
                this.Range,
                0,
                this.CalculatedDamage,
                this.AttackSpeed,
                id_attacker,
                this.Id,
                this.WeaponName,
                true,
                layerMask);
        }
    }
}