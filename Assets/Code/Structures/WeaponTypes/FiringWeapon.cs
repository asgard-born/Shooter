using System.Threading.Tasks;

namespace Structures.WeaponTypes {
    using System;
    using UnityEngine;

    public class FiringWeapon : Weapon, Reloadable {
        public int   MagazineCapacity;
        public float SerialRate;

        public int Ammo;

        [SerializeField] protected Transform aim;

        protected bool isReloading;

        [SerializeField] private float reloadRate;

        public event Action OnAmmoEmpty;

        public bool IsReloading() => this.isReloading;

        protected void Awake() {
            base.Awake();

            this.Ammo = this.MagazineCapacity;
        }

        private void Update() {
            if (this.Ammo == 0) {
                this.Reload();
            }
        }

        public override void Fire() {
            Debug.Log("fire");
        }

        public async Task Reload() {
            this.isReloading = true;
            this.Ammo        = this.MagazineCapacity;
            await Task.Delay(TimeSpan.FromSeconds(this.reloadRate));
            Debug.Log("finish reload");
            this.isReloading = false;
        }
    }
}