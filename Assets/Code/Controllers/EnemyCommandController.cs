namespace Controllers {
    using Structures;
    using UnityEngine.AI;
    using System;
    using Interfaces;
    using UnityEngine;
    using Random = UnityEngine.Random;

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

        private float serialRate;

        public float SerialRate {
            set => this.serialRate = value;
        }

        [Range(17f, 10f)] [SerializeField] private float distanceForNewPos = 15f;

        [SerializeField] private float minDistanceForAttack = 7;
        [SerializeField] private float maxDistanceForAttack = 15;

        private float chosenDistanceForAttack;

        private NavMeshAgent navMeshAgent;
        private AIPhase      phase = AIPhase.ChasingPlayer;
        private Transform    PlayerT;

        private bool needToSafeMyself;

        private Action<AIPhase> OnPhaseChanged;

        public void Initialize(Transform playerT) => this.PlayerT = playerT;

        private void Awake() {
            this.navMeshAgent = this.GetComponent<NavMeshAgent>();

            this.OnPhaseChanged = this.ChangeThePhase;
        }

        private void Update() {
            if ((this.PlayerT.position - this.transform.position).magnitude <= this.chosenDistanceForAttack) {
                this.OnPhaseChanged?.Invoke(AIPhase.Attacking);
            }
            else {
                this.phase = AIPhase.ChasingPlayer;
            }

//            this.BehaveAccordingThePhase();
        }

        private void ChangeThePhase(AIPhase phase) {
            switch (this.phase) {
                case AIPhase.Attacking:

                    this.navMeshAgent.Stop();

                    break;

                case AIPhase.ChasingPlayer:
                    this.chosenDistanceForAttack = Random.Range(this.minDistanceForAttack, this.maxDistanceForAttack);


                    break;

                case AIPhase.Dodging:


                    break;
            }

            this.phase = phase;
        }

        private void BehaveAccordingThePhase() {
            switch (this.phase) {
                case AIPhase.Attacking:
                    this.navMeshAgent.Stop();


                    // fire
                    this.OnFireOnce?.Invoke();

                    break;

                case AIPhase.ChasingPlayer:
                    var randomPosition = this.PlayerT.forward * this.distanceForNewPos
                      + this.PlayerT.right * Random.Range(-this.distanceForNewPos, this.distanceForNewPos);

                    this.navMeshAgent.SetDestination(randomPosition);


                    break;

                case AIPhase.Dodging:
                    break;
            }
        }

        private void ChasingPlayer() {
        }
    }
}