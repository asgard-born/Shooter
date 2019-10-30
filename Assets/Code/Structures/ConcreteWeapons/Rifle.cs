namespace Structures.ConcreteWeapons {
    using UnityEngine;
    using WeaponTypes;

    public class Rifle : FiringWeapon, Reloadable {
        
        public bool IsReloading() => this.isReloading;
        
        public override void Fire() {
            base.Fire();

            if (this.Ammo > 0 && !this.isReloading) {
                RaycastHit hit;

                // if there is some object on the weapon range distance - it became a target and rotation point
                if (Physics.Raycast(this.aim.position, this.aim.forward, out hit, this.Range)) {
                    this.fireDirection = (hit.point - this.aim.position).normalized;
                    // otherwise take target and rotation point from max possibility of current weapon range
                }
                else {
                    Vector3 endPoint = this.aim.position + this.aim.forward * this.Range;

                    this.fireDirection = (endPoint - this.aim.position).normalized;
                }

                // Get Bullet from pool
                var bullet = this.poolManager
                                 .GetObject("Bullet", this.aim.position, Quaternion.LookRotation(this.fireDirection))
                                 .GetComponent<Bullet>();

                bullet.Initialize(bullet.transform.position, this.Range, 0, this.Damage, this.AttackSpeed, true);

                this.Ammo--;
            }
        }
    }
}