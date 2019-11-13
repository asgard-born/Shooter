namespace Controllers {
    using Abstract;
    using System.Collections;
    using Structures;
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class AICommandController : MonoBehaviour, ICommandController {
        public float ForwardMoving    { get; }
        public float HorizontalMoving { get; }
        public float RotationX        { get; }
        public float RotationY        { get; }
        public bool  IsSneak          { get; }
        public bool  IsJumping        { get; }

        public event Action OnFireOnce;
        public event Action OnReload;
        public event Action OnChangingWeapon;
        public event Action OnChasingThePlayer;
        
        Coroutine lastFireRoutine;

        private RaycastHit hit;

        public float serialRate;
        public bool  isAiming;

        [SerializeField] private LayerMask checkingForObstaclesLayerMask;

        public float SerialRate {
            set => this.serialRate = value;
        }

        public event Action<AIPhase> OnPhaseChanged;

        [SerializeField] private float minDistanceForAttack = 15;
        [SerializeField] private float maxDistanceForAttack = 25;

        [Range(.5f, 2.5f)] [SerializeField] private float aimingFallibility = 1.8f;

        private float chosenDistanceForAttack;

        private AIPhase   phase = AIPhase.ChasingPlayer;
        private Transform PlayerT;

        private bool isInitialized;

        public void Initialize(Transform playerT) {
            this.OnPhaseChanged          += this.ChangeThePhase;
            this.chosenDistanceForAttack =  Random.Range(this.minDistanceForAttack, this.maxDistanceForAttack);

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
                    if (this.lastFireRoutine != null) {
                        this.StopCoroutine(this.lastFireRoutine);
                    }

                    this.lastFireRoutine = this.StartCoroutine(this.Fire());
                    break;

                case AIPhase.ChasingPlayer:
                    this.StopCoroutine(this.Fire());
                    this.chosenDistanceForAttack = Random.Range(this.minDistanceForAttack, this.maxDistanceForAttack);
                    break;
            }

            this.phase = phase;
        }

        private IEnumerator Fire() {
            while (this.phase == AIPhase.Attacking) {
                yield return new WaitForSeconds(this.serialRate);
                this.isAiming = true;
                var pointForAttack = this.PlayerT.position + this.PlayerT.right * Random.Range(this.aimingFallibility, -this.aimingFallibility);
                this.transform.LookAt(pointForAttack);
                this.OnFireOnce?.Invoke();
            }
        }

        private void BehaveAccordingThePhase() {
            switch (this.phase) {
                case AIPhase.Attacking:
                    break;

                case AIPhase.ChasingPlayer:
                    this.OnChasingThePlayer?.Invoke();
                    break;
            }
        }
    }
}