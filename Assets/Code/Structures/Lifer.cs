namespace Structures {
    using UnityEngine.UI;
    using UnityEngine;

    public class Lifer : MonoBehaviour {
        [SerializeField] private Slider healthBar;
        [SerializeField] private float  maxHealthValue = 100;
        [SerializeField] private float  health;

        private void Awake() {
            this.health = this.maxHealthValue;
        }

        public void Hit(int damage, int id_attacker, int id_weapon, string weaponName) {
            this.health -= damage;

            Debug.Log(health);
            Debug.Log(this.maxHealthValue);

            this.healthBar.value = health / this.maxHealthValue;

            if (this.health <= 0) {
                this.Death(id_attacker, weaponName);
            }
        }

        private void Death(int id_attacker, string weaponName) {
            Debug.Log($"killed by {id_attacker} with: {weaponName}");
            Destroy(this.gameObject);
        }
    }
}