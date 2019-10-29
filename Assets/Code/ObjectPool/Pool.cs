namespace ObjectPool {
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Pool/ObjectPooling")]
    public class Pool {
        private List<PoolObject> objects;
        private Transform        objectsParent;

        public void Initialize(int count, PoolObject sample, Transform objectsParent) {
            this.objects       = new List<PoolObject>();
            this.objectsParent = objectsParent;

            for (int i = 0; i < count; i++) this.AddObject(sample, objectsParent);
        }

        public void AddObject(PoolObject sample, Transform objectsParent) {
            GameObject temp;

            if (sample.gameObject.scene.name == null) {
                temp      = Object.Instantiate(sample.gameObject, objectsParent, true);
                temp.name = sample.name;
            }
            else {
                temp = sample.gameObject;
            }

            this.objects.Add(temp.GetComponent<PoolObject>());
            temp.SetActive(false);
        }

        public PoolObject GetObject() {
            for (int i = 0; i < this.objects.Count; i++) {
                if (this.objects[i].gameObject.activeInHierarchy == false)
                    return this.objects[i];
            }

            this.AddObject(this.objects[0], this.objectsParent);
            this.objects.RemoveAt(0);
            return this.objects[this.objects.Count - 1];
        }
    }
}