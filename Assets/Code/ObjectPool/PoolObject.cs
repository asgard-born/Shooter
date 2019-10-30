namespace ObjectPool {
    using UnityEngine;

    [AddComponentMenu("Pool/PoolObject")]
    public class PoolObject : MonoBehaviour {
        public Transform PoolContainer;
        public void ReturnToPool() {
            this.gameObject.SetActive(false);
            this.gameObject.transform.SetParent(this.PoolContainer);
        }
    }
}