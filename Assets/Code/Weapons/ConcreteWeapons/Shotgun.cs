namespace Weapons.ConcreteWeapons {
    using UnityEngine;
    using WeaponTypes;

    public class Shotgun : FiringWeapon {
        private RaycastHit hit;

        public override void Attack(int id_attacker) {
            if (this.Ammo > 0 && !this.isReloading) {
                for (int i = 0; i < 16; i++) {
                    var firingRotation = this.CalculateFiringRotationWithSplash();

                    var bullet = this.poolManager
                                     .GetObject("Bullet", this.aim.position, firingRotation)
                                     .GetComponent<Bullet>();

                    this.InitializeTheBullet(bullet, id_attacker, this.layerMask);
                }

                this.Ammo--;
            }
        }
    }
}