namespace Controllers {
    using System;
    using System.Threading.Tasks;
    using ObjectPool;
    using Structures;
    using UnityEngine;

    public class GrenadeController : ThrowingController {
        protected override void OnWeaponReachedTarget() {
            this.Blow();
        }

        private async void Blow() {
            Collider[] overlapResults = new Collider[100];

            await Task.Delay(TimeSpan.FromSeconds(this.blowingTime));
            var poolObject = this.weaponTransform.gameObject.GetComponent<PoolObject>();

            if (poolObject != null) {
                this.weaponTransform.gameObject.GetComponent<PoolObject>().ReturnToPool();
            }

            int numFound = Physics.OverlapSphereNonAlloc(this.weaponTransform.position, this.range, overlapResults);

            for (int i = 0; i < numFound; i++) {
                var lifer = overlapResults[i].gameObject.GetComponent<Lifer>();

                if (lifer != null) {
                    Debug.DrawLine(this.weaponTransform.position, lifer.CenterOfMass.position, Color.red, 10f);

                    if (Physics.Linecast(this.weaponTransform.position, lifer.CenterOfMass.position, out this.hit, this.attackingLayerMask)) {
                        lifer = this.hit.transform.gameObject.GetComponent<Lifer>();

                        if (lifer != null) {
                            lifer.Hit(this.damage, this.id_attacker, this.id_weapon, this.weaponName);
                        }
                    }
                }
            }
        }
    }
}