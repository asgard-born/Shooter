namespace Controllers {
    using UnityEngine;

    public class GrenadeController : BallisticController {
        public  Transform sphere;
        private bool      isFly;

        private void Awake() {
            this.currentVelocity = this.Velocity;
        }

        private void FixedUpdate() {
            if (Input.GetKeyDown(KeyCode.R)) {
                if (this.isFly) {
                    this.time            = 0;
                    this.isFly           = false;
                    this.sphere.position = this.Comparer.position;
                }
                else {
                    this.currentVelocity = this.Velocity;

                    this.radianAngle = Mathf.Deg2Rad * this.Angle;
                    this.maxDistance = (this.Velocity * this.Velocity * Mathf.Sin(2 * this.radianAngle)) / this.Gravity;

                    this.isFly = true;
                }
            }

            if (this.isFly) {
                this.time            += Time.fixedDeltaTime;
                this.sphere.position =  this.CalculateNextPoint(this.time);
            }

            var direction = this.sphere.forward;

            if (Physics.SphereCast(this.sphere.position, this.radius, direction, out this.hit, direction.magnitude, this.layerMask)) {
                this.isFly = false;
            }
        }
    }
}