namespace Controllers {
    using UnityEngine;
    using UnityEngine.AI;
    using Abstract;

    public class EnemyMovementController : MovementController {
        private NavMeshAgent navMeshAgent;
        private Vector3      newPosition;

        [Range(30f, 150f)] [SerializeField] private float distanceForwardFactorForNewPosition    = 100f;
        [Range(30f, 250f)] [SerializeField] private float distanceHorizontalFactorForNewPosition = 200f;

        public override void Move() {
            this.navMeshAgent.isStopped = false;
            this.navMeshAgent.speed     = this.ForwardMoving;
            this.navMeshAgent.SetDestination(this.newPosition);
        }

        public override void Stop() {
            this.ForwardMoving          = 0;
            this.navMeshAgent.isStopped = true;
        }

        public void RotateToPlayer(Transform playerT) => this.transform.LookAt(playerT);

        public void CalculateNewPosition(Transform playerT) {
            var isPositiveNumberForward    = (Random.Range(0, 2) == 0);
            var isPositiveNumberHorizontal = (Random.Range(0, 2) == 0);

            var forwardRandom    = Random.Range(this.distanceForwardFactorForNewPosition, this.distanceForwardFactorForNewPosition / 2);
            var horizontalRandom = Random.Range(this.distanceHorizontalFactorForNewPosition, this.distanceHorizontalFactorForNewPosition / 2);

            forwardRandom    = !isPositiveNumberForward ? -forwardRandom : forwardRandom;
            horizontalRandom = !isPositiveNumberHorizontal ? -horizontalRandom : horizontalRandom;

            this.newPosition = playerT.position + playerT.forward * forwardRandom + playerT.right * horizontalRandom;
        }

        public override void RotateTheCharacter() {
        }

        private void Awake() {
            this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        public override bool IsFalling()      => false;
        public override bool IsGrounded()     => true;
        public override bool IsNearToGround() => true;

        public override void SetAimIK(bool value) {
        }
    }
}