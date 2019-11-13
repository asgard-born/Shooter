namespace Managers {
    using System.Collections.Generic;
    using Abilities;
    using UnityEngine;

    public class EnemiesManager : MonoBehaviour {
        public static EnemiesManager Instance;

        public EnemyManager[] Enemies;

        private void Awake() => Instance = this;

        public void Initialize(Transform playerT, List<Ability> abilities) {
            foreach (var enemy in this.Enemies) {
                enemy.Initialize(playerT);
                foreach (var ability in abilities) {
                    enemy.AddAbility(ability);
                }
            }
        }
    }
}