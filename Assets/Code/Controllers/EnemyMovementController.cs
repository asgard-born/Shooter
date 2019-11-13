namespace Controllers {
    using Abstract;

    public class EnemyMovementController : MovementController {
        public override void Move() {
        }
        
        public override bool IsFalling()      => false;
        public override bool IsGrounded()     => true;
        public override bool IsNearToGround() => true;


        public override void RotatePlayer(float value) {
        }

        public override void SetAimIK(bool value) {
        }
    }
}