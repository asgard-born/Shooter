namespace Controllers {
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;
    using ObjectPool;
    using UnityEngine;

    public class GrenadeController : ThrowingController {
        protected override void OnWeaponReachedTarget() => this.Blow();

        [SerializeField] private LayerMask excludingEnemyMask;

        private byte blowCheckers = 3;

        private async void Blow() {
            Collider[] overlapResults = new Collider[100];

            await Task.Delay(TimeSpan.FromSeconds(this.blowingTime));
            var poolObject = this.weaponTransform.gameObject.GetComponent<PoolObject>();

            int numFound = Physics.OverlapSphereNonAlloc(this.weaponTransform.position, this.range, overlapResults);

            for (int i = 0; i < numFound; i++) {
                var lifer = overlapResults[i].gameObject.GetComponent<LifeController>();

                if (lifer != null) {
                    Debug.DrawLine(this.weaponTransform.position, lifer.CenterOfMass.position, Color.red, 10f);

                    if (Physics.Linecast(this.weaponTransform.position, lifer.CenterOfMass.position, out this.hit, this.excludingEnemyMask)) {
                        return;
                    }
                    
                    lifer.Hit(this.damage, this.id_attacker, this.id_weapon, this.weaponName);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(.1f));

            if (poolObject != null) {
                this.weaponTransform.gameObject.GetComponent<PoolObject>().ReturnToPool();
            }
        }
    }
}