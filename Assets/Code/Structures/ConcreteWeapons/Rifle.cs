namespace Structures.ConcreteWeapons {
    using UnityEngine;
    using WeaponTypes;

    public class Rifle : FiringWeapon{

        public override void Fire() {
            base.Fire();

            if (this.Ammo > 0 && !this.isReloading) {
                var firingRotation = Quaternion.LookRotation(this.aim.forward);

                var bullet = this.poolManager
                                 .GetObject("Bullet", this.aim.position, firingRotation)
                                 .GetComponent<Bullet>();

                bullet.Initialize(bullet.transform.position, this.Range, 0, this.Damage, this.AttackSpeed, true);

                this.Ammo--;
            }
        }
    }
}