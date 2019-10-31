namespace Structures.ConcreteWeapons {
    using UnityEngine;
    using WeaponTypes;

    public class Shotgun : FiringWeapon {
        [Range(.05f, .3f)] [SerializeField] private float splash = .1f;

        private RaycastHit hit;

        public override void Attack(int id_attacker, LayerMask layerMask) {
            base.Attack(id_attacker, layerMask);

            if (this.Ammo > 0 && !this.isReloading) {
                for (int i = 0; i < 16; i++) {
                    var verticalRandomDirectionPart   = (this.aim.forward + this.aim.up * Random.Range(-this.splash, this.splash));
                    var horizontalRandomDirectionPart = (this.aim.forward + this.aim.right * Random.Range(-this.splash, this.splash));

                    var direction = verticalRandomDirectionPart + horizontalRandomDirectionPart;

                    var firingRotation = Quaternion.LookRotation(direction);

                    var bullet = this.poolManager
                                     .GetObject("Bullet", this.aim.position, firingRotation)
                                     .GetComponent<Bullet>();

                    this.InitializeTheBullet(bullet, id_attacker, layerMask);
                }

                this.Ammo--;
            }
        }
    }
}