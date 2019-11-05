namespace Structures.ConcreteWeapons {
    using UnityEngine;
    using WeaponTypes;

    public class Rifle : FiringWeapon {
        public override void Attack(int id_attacker) {
            if (this.Ammo > 0 && !this.isReloading) {
                var firingRotation = Quaternion.LookRotation(this.aim.forward);

                var bullet = this.poolManager
                                 .GetObject("Bullet", this.aim.position, firingRotation)
                                 .GetComponent<Bullet>();

                this.InitializeTheBullet(bullet, id_attacker, this.layerMask);

                this.Ammo--;
            }
        }
    }
}