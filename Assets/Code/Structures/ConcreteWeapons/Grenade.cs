namespace Structures.ConcreteWeapons {
    using Controllers;
    using UnityEngine;
    using WeaponTypes;

    public class Grenade : ThrowingWeapon {
        [SerializeField] private GrenadeController grenadeController;

        public override void Attack(int id_attacker, LayerMask layerMask) {
            
            var grenade = this.poolManager
                              .GetObject("Grenade", this.transform.position, this.transform.rotation);
            
            this.grenadeController.StartFlyingProcess(grenade.transform, id_attacker, layerMask);
        }
    }
}