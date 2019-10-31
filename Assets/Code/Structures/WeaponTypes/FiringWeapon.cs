namespace Structures.WeaponTypes {
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public class FiringWeapon : Weapon, Reloadable {
        public int MagazineCapacity;
        public int Ammo;

        [SerializeField] protected Transform aim;
        [SerializeField] protected float     reloadRate;

        protected bool isReloading;
        
        public bool IsReloading => this.isReloading;

        protected new void Awake() {
            base.Awake();
            this.Ammo = this.MagazineCapacity;
        }

        private void Update() {
            if (this.Ammo == 0) {
                this.Reload();
            }
        }

        public override void Attack(int id_attacker, LayerMask layerMask) {
        }

        public async Task Reload() {
            this.isReloading = true;
            this.Ammo        = this.MagazineCapacity;
            await Task.Delay(TimeSpan.FromSeconds(this.reloadRate));
            this.isReloading = false;

            Debug.Log("finish reloading");
        }

        protected void InitializeTheBullet(Bullet bullet, int id_attacker, LayerMask layerMask) {
            bullet.Initialize(
                bullet.transform.position,
                this.Range,
                0,
                this.Damage,
                this.AttackSpeed,
                id_attacker,
                this.Id,
                this.WeaponName,
                true,
                layerMask);
        }
    }
}