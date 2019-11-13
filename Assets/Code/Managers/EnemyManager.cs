namespace Managers {
    using Abstract;
    using Abilities;
    using Structures;
    using Controllers;
    using UnityEngine;

    public class EnemyManager : CharacterManager {
        [HideInInspector] public AICommandController aiCommandController;

        public void Respawn() => this.LifeController.Resurrect();

        private EnemyMovementController enemyMovementController;
        private Transform               playerT;
        private bool                    canUpdateAnimator;
        
        public float SerialRate {
            set => this.aiCommandController.SerialRate = value;
        }

        private new void Start() {
            base.Start();

            this.aiCommandController            =  this.GetComponent<AICommandController>();
            this.aiCommandController.OnFireOnce += this.weaponController.OnFire;
            this.aiCommandController.OnReload   += this.weaponController.Reload;

            this.enemyMovementController = this.movementController as EnemyMovementController;

            this.aiCommandController.OnChasingThePlayer += () => {
                var movingSpeed = this.statManager.CalculateValue(StatType.MovingSpeed, this.movingSpeed);
                this.movementController.ForwardMoving = movingSpeed;

                this.enemyMovementController.CalculateNewPosition(this.playerT);
                this.movementController.Move();
            };

            this.aiCommandController.OnPhaseChanged += phase => {
                switch (phase) {
                    case AIPhase.Attacking:
                        this.aiCommandController.WaitingForFire = this.statManager.CalculateValue(StatType.SerialRate, this.aiCommandController.SerialRate);
                        this.movementController.Stop();
                        break;

                    case AIPhase.ChasingPlayer:
                        this.enemyMovementController.RotateToPlayer(this.playerT);
                        break;
                }
            };

            this.canUpdateAnimator = true;
        }

        private void Update() {
            if (this.canUpdateAnimator) {
                this.UpdateAnimatorState();
            }
        }

        public void Initialize(Transform playerT) {
            this.playerT = playerT;
            this.aiCommandController.Initialize(playerT);
        }

        private void UpdateAnimatorState() {
            this.animatorManager.ForwardMoving    = this.movementController.ForwardMoving;
            this.animatorManager.HorizontalMoving = this.movementController.HorizontalMoving;
        }
    }
}