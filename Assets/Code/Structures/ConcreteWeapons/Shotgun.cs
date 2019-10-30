namespace Structures.ConcreteWeapons {
    using UnityEngine;
    using WeaponTypes;

    public class Shotgun : FiringWeapon {
        [Range(.05f, .3f)] [SerializeField] private float splash = .1f;

        private RaycastHit hit;

        public override void Fire() {
            base.Fire();

            if (this.Ammo > 0 && !this.isReloading) {
                for (int i = 0; i < 16; i++) {
                    Vector3 randomDirection = new Vector3(
                        Random.Range(-this.splash, this.splash),
                        Random.Range(-this.splash, this.splash),
                        0);

                    randomDirection += this.aim.forward;

                    var firingRotation = Quaternion.LookRotation(randomDirection);

                    var bullet = this.poolManager
                                     .GetObject("Bullet", this.aim.position, firingRotation)
                                     .GetComponent<Bullet>();

                    bullet.Initialize(bullet.transform.position, this.Range, 0, this.Damage, this.AttackSpeed, true);
                }

                this.Ammo--;
            }
        }
    }
}