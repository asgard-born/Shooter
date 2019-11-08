namespace Controllers {
    using System;
    using Interfaces;
    using Structures;
    using UnityEngine.AI;
    using UnityEngine;

    public class EnemyCommandController : MonoBehaviour, CommandController {
        public float ForwardMoving    { get; }
        public float HorizontalMoving { get; }
        public float RotateX          { get; }
        public float RotateY          { get; }
        public bool  IsSneak          { get; }
        public bool  IsJumping        { get; }

        public event Action OnFireOnce;
        public event Action OnReload;
        public event Action OnChangingWeapon;

        private Transform PlayerT;
        private AIPhase   phase;
        private bool      needToSafeMyself;
        private float     serialRate;
        private float     distanceForAttack = 15;

        public float SerialRate {
            set => this.serialRate = value;
        }

        [SerializeField] private NavMeshAgent navMeshAgent;

        public void Initialize(Transform playerT) {
            this.PlayerT = playerT;
        }

        public void Update() {
            if ((this.PlayerT.position - this.transform.position).magnitude <= this.distanceForAttack) {
                this.phase = AIPhase.Attacking;
            }
            else {
                this.phase = AIPhase.MovingToPlayer;
            }

            this.BehaveAccordingThePhase();
        }

        private void BehaveAccordingThePhase() {
            if (this.phase == AIPhase.Attacking) {
//                this.
            }
            else if (this.phase == AIPhase.MovingToPlayer) {
            }
            else if (this.phase == AIPhase.Dodging) {
            }
        }
    }
}