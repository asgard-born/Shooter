namespace Managers {
    using Controllers.Abstract;
    using System;
    using System.Threading.Tasks;
    using Controllers;
    using UnityEngine;

    public class CharacterManager : MonoBehaviour {
        public WeaponController WeaponController => this.weaponController;

        [HideInInspector] public LifeController LifeController;

        protected MovementController movementController;

        [SerializeField] protected float movingSpeed = 5f;

        protected AnimatorManager animatorManager;
        protected StatManager     statManager;

        protected WeaponController weaponController;

        public int Id => this.id;

        [SerializeField] private int id;

        public event Action OnDeath;

        public void Respawn() => this.LifeController.Resurrect();

        protected async void Start() {
            this.GetLogicEssences();
            await Task.Delay(TimeSpan.FromSeconds(1));
            this.Initialize();
        }

        private void Initialize() {
            this.LifeController.Initialize(this.statManager);
            this.weaponController.Initialize(this.statManager);
        }

        protected void Awake() {
            this.LifeController         =  this.GetComponent<LifeController>();
            this.LifeController.OnDeath += () => this.OnDeath?.Invoke();
        }

        private void GetLogicEssences() {
            this.statManager        = this.GetComponent<StatManager>();
            this.weaponController   = this.GetComponent<WeaponController>();
            this.animatorManager    = this.GetComponent<AnimatorManager>();
            this.movementController = this.GetComponent<MovementController>();
        }
    }
}