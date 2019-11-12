namespace Structures {
    using Abilities.Interfaces;
    using UnityEngine.Assertions;
    using System;
    using UnityEngine.UI;
    using UnityEngine;

    public class Lifer : MonoBehaviour {
        public Transform CenterOfMass;

        [SerializeField] private Slider     healthBar;
        [SerializeField] private GameObject visiblePart;
        [SerializeField] private float      maxHealthValue = 100;
        [SerializeField] private float      health;

        public float DamageFactorAdditive;
        public int   DamageFactorMultiplicative = 1;

        public event Action OnDeath;

        public void Respawn() {
            this.visiblePart.SetActive(true);
            this.health          = this.maxHealthValue;
            this.healthBar.value = this.health / this.maxHealthValue;
        }

        private float CalculateDamageValue(float damage) {
            if (this.DamageFactorMultiplicative == 0) {
                this.DamageFactorMultiplicative = 1;
            }
            
            damage = this.DamageFactorMultiplicative > 1 ? damage * this.DamageFactorMultiplicative : damage / this.DamageFactorMultiplicative;
            damage += damage * this.DamageFactorAdditive;

            return damage;
        }

        public void Hit(float damage, int id_attacker, int id_weapon, string weaponName) {
            damage = this.CalculateDamageValue(damage);

            this.health -= damage;

            this.healthBar.value = this.health / this.maxHealthValue;

            if (this.health <= 0) {
                this.Death(id_attacker, weaponName);
            }
        }

        private void Awake() => this.health = this.maxHealthValue;

        private void OnValidate() => Assert.IsNotNull(this.CenterOfMass);

        private void Update()
            => this.healthBar.transform.LookAt(this.healthBar.transform.position + Camera.main.transform.rotation * Vector3.forward);

        private void Death(int id_attacker, string weaponName) {
            Debug.Log($"killed by {id_attacker} with: {weaponName}");

            this.visiblePart.SetActive(false);
            this.OnDeath?.Invoke();
        }
    }
}