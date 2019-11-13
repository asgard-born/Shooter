namespace Weapons.Abstract {
    using System;
    using System.Threading.Tasks;
    using Abilities;
    using Managers;
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public GameObject WeaponObject;

        public int    Id;
        public string WeaponName;
        public float  SerialRate;
        public int    Damage;
        public float  Range;
        public Sprite Sprite;

        protected float CalculatedDamage
            => this.statManager.CalculateValue(StatType.AttackDamage, this.Damage);

        protected StatManager statManager;
        protected PoolManager poolManager;

        [SerializeField] protected LayerMask layerMask;

        public abstract void Attack(int id_attacker);

        public void Initialize(StatManager statManager) => this.statManager = statManager;

        protected async void Awake() {
            this.poolManager = PoolManager.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }
        }

        public bool IsActive { get; set; }
    }
}