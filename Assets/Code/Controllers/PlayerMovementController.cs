namespace Controllers {
    using Abstract;
    using System;
    using UnityEngine;
    using System.Threading.Tasks;
    using RootMotion.FinalIK;

    public class PlayerMovementController : MovementController {
        [SerializeField] private float jumpVelocity = 7f;

        private Rigidbody  rigidbody;
        private AimIK      aimIK;
        private RaycastHit hit;

        private readonly float nearGroundMarker = 1.2f;

        public override void SetAimIK(bool isSet) => this.aimIK.enabled = isSet;

        // Should be called in Update
        public override void RotateTheCharacter() =>
            this.transform.rotation = Quaternion.Euler(0, this.RotationX, 0);

        // Should be called in FixedUpdate
        public override void Move() {
            this.rigidbody.MovePosition(
                this.transform.position +
                (this.transform.right * 2 * this.HorizontalMoving + this.transform.forward * this.ForwardMoving));
        }

        public override void Stop() {
        }

        public override bool IsFalling() => !this.IsGrounded() && this.rigidbody.velocity.y < 0;

        public override bool IsGrounded()
            => Physics.Raycast(
                this.transform.position + new Vector3(0, 0.2f, 0),
                Vector3.down,
                out this.hit,
                0.2f);

        public override bool IsNearToGround()
            => (!this.IsGrounded()) && Physics.Raycast(
                this.transform.position,
                Vector3.down,
                out this.hit,
                this.nearGroundMarker);

        private void Awake() {
            this.aimIK     = this.GetComponent<AimIK>();
            this.rigidbody = this.GetComponent<Rigidbody>();
        }

        private void LateUpdate() {
            if (this.IsGrounded() && this.IsJumpPressed) {
                this.StartJumping();
                this.IsJumping = true;
            }

            if (this.IsFalling() && this.IsNearToGround()) {
                this.UpdateJumpChecker();
            }
        }

        private async void UpdateJumpChecker() {
            await Task.Delay(TimeSpan.FromSeconds(0.7f));
            this.IsJumping = false;
        }

        private void StartJumping() => this.rigidbody.velocity = new Vector3(this.HorizontalMoving, this.jumpVelocity, this.ForwardMoving);
    }
}