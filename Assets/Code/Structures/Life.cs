namespace Structures {
    using UnityEngine;

    public class Life : MonoBehaviour {
        private int life = 100;

        public void GetDamage(int value) {
            this.life -= value;
        }

        private void Update() {
            if (this.life <= 0) {
                this.Death();
            }
        }

        private void Death() {
            GameObject.Destroy(this.gameObject);
        }
    }
}