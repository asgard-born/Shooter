namespace Managers {
    using System;
    using System.Threading.Tasks;
    using ScriptableObjects;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        public  PoolOptions PoolOptions;
        private PoolManager poolManager;

        private async void Awake() {
            // Get Singletones

            this.poolManager = PoolManager.Instance;

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }

            this.poolManager.Initialize(this.PoolOptions.Pools);
        }
    }
}