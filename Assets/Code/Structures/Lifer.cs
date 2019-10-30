namespace Structures {
    using UnityEngine;

    public class Lifer : MonoBehaviour {
        [SerializeField] private int life = 100;

        public void Hit(int damage, int id_attacker, int id_weapon, string weaponName) {
            this.life -= damage;

            if (this.life <= 0) {
                this.Death(id_attacker, weaponName);
            }
        }

        private void Death(int id_attacker, string weaponName) {
            Debug.Log($"killed by {id_attacker} with: {weaponName}");
            Destroy(this.gameObject);
        }
    }
}