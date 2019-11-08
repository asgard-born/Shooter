namespace Managers {
    using Structures;
    using UnityEngine;

    public class EnemiesManager : MonoBehaviour {
        public static EnemiesManager Instance;

        [SerializeField] private Enemy[] enemies;

        private void Awake() => Instance = this;

        public void Initialize(Transform playerT) {
            foreach (var enemy in this.enemies) {
                enemy.enemyCommandController.Initialize(playerT);
            }
        }
    }
}