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
        
        public bool IsReloading() => this.isReloading;

        protected new void Awake() {
            base.Awake();
            this.Ammo = this.MagazineCapacity;
        }

        private void Update() {
            if (this.Ammo == 0) {
                this.Reload();
            }
        }

        public override void Fire() {
        }

        public async Task Reload() {
            this.isReloading = true;
            this.Ammo        = this.MagazineCapacity;
            await Task.Delay(TimeSpan.FromSeconds(this.reloadRate));
            this.isReloading = false;

            Debug.Log("finish reloading");
        }
    }
}