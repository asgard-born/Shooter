namespace Controllers {
    using System;
    using Abilities;
    using Managers;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.UI;

    public class LifeController : MonoBehaviour {
        public Transform CenterOfMass;

        private StatManager statManager;

        [SerializeField] private Slider     healthBar;
        [SerializeField] private GameObject visiblePart;
        [SerializeField] private float      maxHealthValue = 100;
        [SerializeField] private float      health;

        public event Action OnDeath;

        public void Resurrect() {
            this.visiblePart.SetActive(true);
            this.health          = this.maxHealthValue;
            this.healthBar.value = this.health / this.maxHealthValue;
        }

        public void Initialize(StatManager statManager) => this.statManager = statManager;

        public void Hit(float damage, int id_attacker, int id_weapon, string weaponName) {
            damage = this.statManager.CalculateValue(StatType.IncomingDamage, damage);

            this.health -= damage;

            this.healthBar.value = this.health / this.maxHealthValue;

            if (this.health <= 0) {
                this.Kill(id_attacker, weaponName);
            }
        }

        private void Awake() => this.health = this.maxHealthValue;

        private void OnValidate() => Assert.IsNotNull(this.CenterOfMass);

        private void Update()
            => this.healthBar.transform.LookAt(this.healthBar.transform.position + Camera.main.transform.rotation * Vector3.forward);

        private void Kill(int id_attacker, string weaponName) {
            Debug.Log($"killed by {id_attacker} with: {weaponName}");

            this.visiblePart.SetActive(false);
            this.OnDeath?.Invoke();
        }
    }
}