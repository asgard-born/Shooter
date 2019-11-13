namespace Controllers {
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;
    using ObjectPool;
    using UnityEngine;

    public class GrenadeController : ThrowingController {
        protected override void OnWeaponReachedTarget() => this.Blow();

        private List<LifeController> didNotReachedByTheRay;

        private byte blowCheckers = 3;

        private async void Blow() {
            Collider[] overlapResults = new Collider[100];

            await Task.Delay(TimeSpan.FromSeconds(this.blowingTime));
            var poolObject = this.weaponTransform.gameObject.GetComponent<PoolObject>();

            int numFound = Physics.OverlapSphereNonAlloc(this.weaponTransform.position, this.range, overlapResults);

            this.didNotReachedByTheRay = new List<LifeController>(numFound);

            for (int i = 0; i < numFound; i++) {
                var lifer = overlapResults[i].gameObject.GetComponent<LifeController>();

                if (lifer != null) {
                    Debug.DrawLine(this.weaponTransform.position, lifer.CenterOfMass.position, Color.red, 10f);

                    if (Physics.Linecast(this.weaponTransform.position, lifer.CenterOfMass.position, out this.hit, this.attackingLayerMask)) {
                        var catchedLifer = this.hit.transform.gameObject.GetComponent<LifeController>();

                        if (lifer != catchedLifer) {
                            this.didNotReachedByTheRay.Add(lifer);
                        }

                        if (catchedLifer != null) {
                            catchedLifer.Hit(this.damage, this.id_attacker, this.id_weapon, this.weaponName);
                        }
                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(.1f));

            for (int i = 1; i <= this.blowCheckers; i++) {
                DoNewLinecastIfPreviousWasBlockedByAnotherLifer();
            }

            void DoNewLinecastIfPreviousWasBlockedByAnotherLifer() {
                if (this.didNotReachedByTheRay.Count > 0) {
                    var candidatesForDoubleCheck = new List<LifeController>(this.didNotReachedByTheRay.Count);

                    foreach (var doubleCheckedLifer in this.didNotReachedByTheRay) {
                        if (Physics.Linecast(this.weaponTransform.position, doubleCheckedLifer.CenterOfMass.position, out this.hit, this.attackingLayerMask)) {
                            var catchedLifer = this.hit.transform.gameObject.GetComponent<LifeController>();

                            if (doubleCheckedLifer != catchedLifer) {
                                candidatesForDoubleCheck.Add(doubleCheckedLifer);
                            }

                            if (catchedLifer != null) {
                                catchedLifer.Hit(this.damage, this.id_attacker, this.id_weapon, this.weaponName);
                            }
                        }
                    }

                    this.didNotReachedByTheRay.Clear();

                    if (candidatesForDoubleCheck.Count > 0) {
                        this.didNotReachedByTheRay.AddRange(candidatesForDoubleCheck);
                    }
                }
            }

            if (poolObject != null) {
                this.weaponTransform.gameObject.GetComponent<PoolObject>().ReturnToPool();
            }
        }
    }
}