namespace ScriptableObjects {
    using ObjectPool;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PoolOptions")]
    public class PoolOptions : ScriptableObject {
        public PoolInstance[] Pools;

        private void OnValidate() {
            for (int i = 0; i < this.Pools.Length; i++) {
                this.Pools[i].Name = this.Pools[i].Prefab.name;
            }
        }
    }
}