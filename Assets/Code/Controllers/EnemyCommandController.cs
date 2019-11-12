namespace Controllers {
    using System.Collections;
    using Structures;
    using UnityEngine.AI;
    using System;
    using Interfaces;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EnemyCommandController : MonoBehaviour, CommandController {
        public float ForwardMoving    => this.forwardMoving;
        public float HorizontalMoving { get; }
        public float RotateX          { get; }
        public float RotateY          { get; }
        public bool  IsSneak          { get; }
        public bool  IsJumping        { get; }

        public event Action OnFireOnce;
        public event Action OnReload;
        public event Action OnChangingWeapon;

        private Lifer      life;
        private RaycastHit hit;

        public float serialRate;
        public bool  isAiming;

        [SerializeField] private LayerMask checkingForObstaclesLayerMask;

        public float SerialRate {
            set => this.serialRate = value;
        }

        [Range(30f, 150f)] [SerializeField] private float distanceForwardFactorForNewPosition = 100f;
        [Range(30f, 250f)] [SerializeField] private float distanceHorizontalFactorForNewPosition = 200f;

        [SerializeField] private float minDistanceForAttack = 15;
        [SerializeField] private float maxDistanceForAttack = 25;

        [Range(.5f, 2.5f)] [SerializeField] private float aimingFallibility = 1.8f;

        private float chosenDistanceForAttack;

        private NavMeshAgent navMeshAgent;
        private AIPhase      phase = AIPhase.ChasingPlayer;
        private Transform    PlayerT;

        private bool isInitialized;

        private Action<AIPhase> OnPhaseChanged;
        private float           forwardMoving;

        public void Initialize(Transform playerT) {
            this.life         = this.GetComponent<Lifer>();
            this.navMeshAgent = this.GetComponent<NavMeshAgent>();

            this.OnPhaseChanged          = this.ChangeThePhase;
            this.chosenDistanceForAttack = Random.Range(this.minDistanceForAttack, this.maxDistanceForAttack);

            this.PlayerT       = playerT;
            this.isInitialized = true;
        }

        private void Update() {
            if (this.isInitialized) {
                this.UpdateBehaviour();

                this.BehaveAccordingThePhase();
            }
        }

        private void UpdateBehaviour() {
            if (!this.isInitialized) return;

            var canAttack = this.CheckForAttackOpportunity();

            if (this.phase != AIPhase.Attacking && canAttack && (this.PlayerT.position - this.transform.position).magnitude <= this.chosenDistanceForAttack) {
                this.OnPhaseChanged?.Invoke(AIPhase.Attacking);
            }
            else if (!canAttack
             || this.phase != AIPhase.ChasingPlayer && (this.PlayerT.position - this.transform.position).magnitude > this.chosenDistanceForAttack) {
                this.OnPhaseChanged?.Invoke(AIPhase.ChasingPlayer);
            }
        }

        private bool CheckForAttackOpportunity() {
            return !Physics.Linecast(this.transform.position + Vector3.up, this.PlayerT.position + Vector3.up, out this.hit, this.checkingForObstaclesLayerMask);
        }

        private void ChangeThePhase(AIPhase phase) {
            this.phase = phase;

            switch (phase) {
                case AIPhase.Attacking:
                    this.forwardMoving          = 0;
                    this.navMeshAgent.isStopped = true;
                    this.StopCoroutine(this.Fire());
                    this.StartCoroutine(this.Fire());
                    break;

                case AIPhase.ChasingPlayer:
                    this.chosenDistanceForAttack = Random.Range(this.minDistanceForAttack, this.maxDistanceForAttack);
                    break;
            }

            this.phase = phase;
        }

        private IEnumerator Fire() {
            while (this.phase == AIPhase.Attacking) {
                this.isAiming = true;
                var pointForAttack = this.PlayerT.position + this.PlayerT.right * Random.Range(this.aimingFallibility, -this.aimingFallibility);
                this.transform.LookAt(pointForAttack);
                this.OnFireOnce?.Invoke();
                yield return new WaitForSeconds(this.serialRate);
            }
        }

        private void BehaveAccordingThePhase() {
            switch (this.phase) {
                case AIPhase.Attacking:

                    break;

                case AIPhase.ChasingPlayer:
                    this.ChasingPlayer();

                    break;
            }
        }

        private void ChasingPlayer() {
            var isPositiveNumberForward    = (Random.Range(0, 2) == 0);
            var isPositiveNumberHorizontal = (Random.Range(0, 2) == 0);

            var forwardRandom    = Random.Range(this.distanceForwardFactorForNewPosition, this.distanceForwardFactorForNewPosition / 2);
            var horizontalRandom = Random.Range(this.distanceHorizontalFactorForNewPosition, this.distanceHorizontalFactorForNewPosition / 2f);

            forwardRandom    = !isPositiveNumberForward ? -forwardRandom : forwardRandom;
            horizontalRandom = !isPositiveNumberHorizontal ? -horizontalRandom : horizontalRandom;

            var randomPosition = this.PlayerT.position
              + this.PlayerT.forward * forwardRandom + this.PlayerT.right * horizontalRandom;

            this.transform.LookAt(this.PlayerT);
            this.forwardMoving = 1f;

            this.navMeshAgent.isStopped = false;
            this.navMeshAgent.SetDestination(randomPosition);
        }
    }
}