namespace Structures.WeaponTypes {
    using System;
    using UnityEngine;

    public class FiringWeapon : Weapon, Reloadable {
        [HideInInspector] public int Ammo;

        public event Action OnAmmoEmpty;

        protected void Awake() {
            base.Awake();

            this.Ammo = this.MagazineCapacity;
        }

        public override void Fire() {
            if (this.Ammo > 0) {
                this.Ammo--;
            }
            else {
                this.OnAmmoEmpty?.Invoke();
            }
        }

        public void Reload() {
            
        }
    }
}