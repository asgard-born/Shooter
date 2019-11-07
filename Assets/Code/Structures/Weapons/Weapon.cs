namespace Structures.Weapons {
    using System;
    using System.Threading.Tasks;
    using Managers;
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public GameObject WeaponObject;

        [SerializeField] protected LayerMask layerMask;

        public int    Id;
        public string WeaponName;
        public float  SerialRate;
        public int    Damage;
        public float  Range;
        public Sprite Sprite;

        protected Vector3     fireDirection;
        protected PoolManager poolManager;

        protected async void Awake() {
            this.poolManager = PoolManager.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }
        }

        public abstract void Attack(int id_attacker);
    }
}