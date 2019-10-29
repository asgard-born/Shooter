namespace ObjectPool {
    using UnityEngine;

    [AddComponentMenu("Pool/PoolObject")]
    public class PoolObject : MonoBehaviour {
        public void ReturnToPool() {
            this.gameObject.SetActive(false);
            this.gameObject.transform.SetParent(null);
        }
    }
}