using System;
using UnityEngine.Assertions;

namespace Structures {
    using UnityEngine.UI;
    using UnityEngine;

    public class Lifer : MonoBehaviour {
        public Transform CenterOfMass;
        [SerializeField] private Slider healthBar;
        [SerializeField] private float  maxHealthValue = 100;
        [SerializeField] private float  health;

        public void Hit(int damage, int id_attacker, int id_weapon, string weaponName) {
            this.health -= damage;

            Debug.Log(health);
            Debug.Log(this.maxHealthValue);

            this.healthBar.value = health / this.maxHealthValue;

            if (this.health <= 0) {
                this.Death(id_attacker, weaponName);
            }
        }
        
        private void Awake() {
            this.health = this.maxHealthValue;
        }

        private void OnValidate() {
            Assert.IsNotNull(this.CenterOfMass);
        }

        private void Update() {
            this.healthBar.transform.LookAt(this.healthBar.transform.position + Camera.main.transform.rotation * Vector3.forward);
        }

        private void Death(int id_attacker, string weaponName) {
            Debug.Log($"killed by {id_attacker} with: {weaponName}");
            Destroy(this.gameObject);
        }
    }
}