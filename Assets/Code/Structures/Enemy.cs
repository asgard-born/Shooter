namespace Structures {
    using Managers;
    using Controllers;
    using UnityEngine;

    public class Enemy : Character {
        [HideInInspector] public EnemyCommandController EnemyCommandController;
        [HideInInspector] public WeaponController       WeaponController;
        private                  MovementController     movementController;
        private                  AnimatorManager        animatorManager;

        public void Respawn() => this.lifer.Respawn();

        private bool canUpdateAnimator;

        private void Awake() {
            this.WeaponController       = this.GetComponent<WeaponController>();
            this.movementController     = this.GetComponent<MovementController>();
            this.animatorManager        = this.GetComponent<AnimatorManager>();
            this.EnemyCommandController = this.GetComponent<EnemyCommandController>();

            this.EnemyCommandController.OnFireOnce += this.WeaponController.OnFire;
            this.EnemyCommandController.OnReload   += this.WeaponController.Reload;

            this.canUpdateAnimator = true;
            this.WeaponController.Initialize();
        }

        private void Update() {
            if (canUpdateAnimator) {
                this.UpdateAnimatorState();
            }
        }

        public void Initialize(Transform playerT) => this.EnemyCommandController.Initialize(playerT);

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving    = this.EnemyCommandController.ForwardMoving;
            this.animatorManager.HorizontalMoving = this.EnemyCommandController.HorizontalMoving;

            this.animatorManager.IsJumping      = this.movementController.IsJumping;
            this.animatorManager.IsFalling      = this.movementController.IsFalling();
            this.animatorManager.IsGrounded     = this.movementController.IsGrounded();
            this.animatorManager.IsNearToGround = this.movementController.IsNearToGround();
        }
    }
}