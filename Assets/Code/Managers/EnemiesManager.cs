namespace Managers {
    using Controllers;
    using UnityEngine;

    public class EnemiesManager : MonoBehaviour {
        public static EnemiesManager Instance;

        [SerializeField] private EnemyCommandController[] enemies;

        private void Awake() => Instance = this;

        public void Initialize(Transform playerT) {
            foreach (var enemy in this.enemies) {
                enemy.Initialize(playerT);
            }
        }
    }
}