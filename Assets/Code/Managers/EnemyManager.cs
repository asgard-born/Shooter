namespace Managers {
    using Controllers;
    using UnityEngine;

    public class EnemyManager : CharacterManager {
        [HideInInspector] public EnemyCommandController EnemyCommandController;

        public void Respawn() => this.LifeController.Resurrect();

        private bool canUpdateAnimator;

        private new void Start() {
            base.Start();
            this.EnemyCommandController = this.GetComponent<EnemyCommandController>();
            this.EnemyCommandController.OnFireOnce += this.weaponController.OnFire;
            this.EnemyCommandController.OnReload   += this.weaponController.Reload;

            this.canUpdateAnimator = true;
        }

        private void Update() {
            if (this.canUpdateAnimator) {
                this.UpdateAnimatorState();
            }
        }

        public void Initialize(Transform playerT) {
            this.EnemyCommandController.Initialize(playerT);
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving    = this.EnemyCommandController.ForwardMoving;
            this.animatorManager.HorizontalMoving = this.EnemyCommandController.HorizontalMoving;
        }
    }
}