using System;
using UnityEngine;

namespace Structures.WeaponTypes {
    public class FiringWeapon : Weapon {
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
        
        
    }
}