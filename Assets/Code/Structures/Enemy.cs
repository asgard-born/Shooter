namespace Structures {
    using Controllers;
    using UnityEngine;

    public class Enemy : MonoBehaviour {
        public EnemyCommandController enemyCommandController;

        private void Awake() {
            this.enemyCommandController = this.GetComponent<EnemyCommandController>();
        }
    }
}