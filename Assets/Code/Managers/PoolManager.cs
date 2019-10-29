namespace Managers {
    using System;
    using ObjectPool;
    using UnityEngine;

    public class PoolManager : MonoBehaviour {
        public Transform PoolParent;

        public static PoolManager Instance;

        private PoolInstance[] pools;

        private void Awake() => Instance = this;

        public void Initialize(PoolInstance[] newPools) {
            this.pools = newPools;

            for (int i = 0; i < this.pools.Length; i++) {
                if (this.pools[i].Prefab != null) {
                    this.pools[i].Pool = new Pool();
                    this.pools[i].Pool.Initialize(this.pools[i].Count, this.pools[i].Prefab, this.PoolParent);
                }
            }
        }

        public GameObject GetObject(string name, Vector3 position, Quaternion rotation) {
            if (this.pools != null) {
                for (int i = 0; i < this.pools.Length; i++) {
                    if (String.CompareOrdinal(this.pools[i].Name, name) == 0) {
                        var result = this.pools[i].Pool.GetObject().gameObject;
                        result.transform.position = position;
                        result.transform.rotation = rotation;
                        result.SetActive(true);
                        return result;
                    }
                }
            }

            return null;
        }
    }
}