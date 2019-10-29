namespace Controllers {
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public class MovementController : MonoBehaviour {
        public static MovementController Instance;

        [HideInInspector] public float HorizontalMoving;
        [HideInInspector] public float ForwardMoving;
        [HideInInspector] public bool  IsJumpPressed;
        [HideInInspector] public bool  IsJumping;

        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform characterTransform;

        [SerializeField] private float movingSpeed;
        [SerializeField] private float jumpVelocity = 7f;

        private readonly float nearGroundMarker = 1.2f;

        private float turnSmoothVelocity;

        private void Awake() => Instance = this;

        // Should be called in Update
        public void RotatePlayer(float mouseX) =>
            this.characterTransform.rotation = Quaternion.Euler(0, mouseX, 0);

        // Should be called in FixedUpdate
        public void Move(float forwardMoving, float horizontalMoving) {
            forwardMoving    *= this.movingSpeed;
            horizontalMoving *= this.movingSpeed;

            this.rigidbody.MovePosition(
                this.characterTransform.position +
                (this.characterTransform.right * horizontalMoving + this.characterTransform.forward * forwardMoving));
        }

        public bool IsFalling() => !this.IsGrounded() && this.rigidbody.velocity.y < 0;

        public bool IsGrounded() {
            RaycastHit hit;
            return Physics.Raycast(this.characterTransform.position + new Vector3(0, 0.2f, 0), Vector3.down, out hit, 0.2f);
        }

        public bool IsNearToGround() {
            RaycastHit hit;
            return (!this.IsGrounded()) && Physics.Raycast(this.characterTransform.position, Vector3.down, out hit, this.nearGroundMarker);
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