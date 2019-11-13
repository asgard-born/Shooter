namespace Weapons.ConcreteWeapons {
    using Controllers;
    using UnityEngine;
    using WeaponTypes;

    public class Grenade : ThrowingWeapon {
        [SerializeField] private ThrowingController throwingController;

        public float BlowingTime;

        public override void Attack(int id_attacker) {
            var grenade = this.poolManager.GetObject("Grenade", this.transform.position, this.transform.rotation);

            this.throwingController.StartFlyingProcess(
                grenade.transform,
                this.CalculatedDamage,
                this.Range,
                this.BlowingTime,
                id_attacker,
                this.Id,
                this.WeaponName,
                this.layerMask);
        }
    }
}