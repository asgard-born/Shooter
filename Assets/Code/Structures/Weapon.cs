namespace Structures {
    using System;
    using System.Threading.Tasks;
    using Managers;
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public GameObject WeaponObject;

        public int    Id;
        public string WeaponName;
        public int    Damage;
        public int    MagazineCapacity;
        public int    Speed;
        public float  Range;

        public float SerialRate;
        public float ReloadRate;

        [SerializeField] protected Transform aim;

        protected Vector3 fireDirection;

        protected PoolManager poolManager;

        protected async void Awake() {
            this.poolManager = PoolManager.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }
        }

        public abstract void Fire();
    }
}