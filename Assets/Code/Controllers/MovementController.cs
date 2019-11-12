﻿namespace Controllers {
    using System;
    using UnityEngine;
    using System.Threading.Tasks;
    using RootMotion.FinalIK;

    public class MovementController : MonoBehaviour {
        public static MovementController Instance;

        public float SpeedFactorAdditive;
        public int   SpeedFactorMultiplicative = 1;

        public float MovingSpeed;

        [HideInInspector] public float HorizontalMoving;
        [HideInInspector] public float ForwardMoving;
        [HideInInspector] public bool  IsJumpPressed;
        [HideInInspector] public bool  IsJumping;

        [SerializeField] private float jumpVelocity      = 7f;
        [SerializeField] private float jumpRotationSpeed = 1.2f;

        private readonly float turnSmoothingTimer = 0.2f;

        private readonly float nearGroundMarker = 1.2f;

        private Rigidbody rigidbody;
        private AimIK     aimIK;
        private float     turnSmoothVelocity;

        public void SetAimIK(bool isSet) => this.aimIK.enabled = isSet;

        // Should be called in Update
        public void RotatePlayer(float mouseX) =>
            this.transform.rotation = Quaternion.Euler(0, mouseX, 0);

        // Should be called in FixedUpdate
        public void Move(float forwardMoving, float horizontalMoving) {
            forwardMoving    *= this.MovingSpeed * Time.fixedDeltaTime;
            horizontalMoving *= this.MovingSpeed * Time.fixedDeltaTime;

            this.rigidbody.MovePosition(
                this.transform.position +
                (this.transform.right * horizontalMoving + this.transform.forward * forwardMoving));
        }

        public bool IsFalling() => !this.IsGrounded() && this.rigidbody.velocity.y < 0;

        public bool IsGrounded() {
            RaycastHit hit;

            return Physics.Raycast(this.transform.position + new Vector3(0, 0.2f, 0), Vector3.down, out hit, 0.2f);
        }

        public bool IsNearToGround() {
            RaycastHit hit;

            return (!this.IsGrounded()) && Physics.Raycast(this.transform.position, Vector3.down, out hit, this.nearGroundMarker);
        }

        public void CalculateMovingValue() {
            if (this.SpeedFactorMultiplicative == 0) {
                this.SpeedFactorMultiplicative = 1;
                
            }
            this.MovingSpeed = this.SpeedFactorMultiplicative > 0
                ? this.MovingSpeed * this.SpeedFactorMultiplicative
                : this.MovingSpeed / this.SpeedFactorMultiplicative;


            this.MovingSpeed += this.MovingSpeed * this.SpeedFactorAdditive;
        }

        private void Awake() {
            Instance = this;

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