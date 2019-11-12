namespace Weapons {
    using Abilities.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Abilities;
    using Managers;
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour, IActivableBuffable {
        public GameObject WeaponObject;

        #region abilities
        public Dictionary<AbilityType, Ability> Abilities => this.abilities;
        
        public void AddAbility(Ability ability) {
            
        }

        public void RemoveAbility(Ability ability) {
            
        }

        public void EnableAbility(Ability ability) {
            
        }

        public void DisableAbility(Ability ability) {
            
        }
        #endregion abilities

        public int    Id;
        public string WeaponName;
        public float  SerialRate;
        public int    Damage;
        public float  Range;
        public Sprite Sprite;

        protected Vector3     fireDirection;
        protected PoolManager poolManager;

        [SerializeField] protected LayerMask layerMask;

        private readonly Dictionary<AbilityType, Ability> abilities = new Dictionary<AbilityType, Ability>();

        public abstract void Attack(int id_attacker);

        protected async void Awake() {
            this.poolManager = PoolManager.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }
        }

        public bool IsActive { get; set; }
        public event Action OnActivated;
        public event Action OnInactivated;
    }
}