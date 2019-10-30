namespace Structures.WeaponTypes {
    using System;
    using UnityEngine;

    public class FiringWeapon : Weapon, Reloadable {
        public int   MagazineCapacity;
        public float SerialRate;

        [SerializeField] private float reloadRate;

        [HideInInspector] public int Ammo;

        [SerializeField] protected Transform aim;

        public event Action OnAmmoEmpty;

        public float GetReloadRate() => this.reloadRate;

        protected void Awake() {
            base.Awake();

            this.Ammo = this.MagazineCapacity;
        }

        public override void Fire() {
            if (this.Ammo == 0) {
                this.OnAmmoEmpty?.Invoke();
            }
        }

        public void Reload() {
            this.Ammo = this.MagazineCapacity;
        }
    }
}