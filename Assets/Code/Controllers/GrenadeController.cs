namespace Controllers {
    using Managers;
    using UnityEngine;

    public class GrenadeController : BallisticController {
        public  Transform sphere;
        private bool      isFly;

        protected override Vector3 CalculateNextPoint(float timepoint) {
            var arcPoint2D = PhysicsMath.CalculateArcPoint2D(this.radianAngle, this.Gravity, this.currentVelocity, timepoint, this.maxDistance);
            var offset     = new Vector3(0, arcPoint2D.y, arcPoint2D.x);

            return this.Comparer.position + this.Comparer.rotation * offset;
        }

        private void Awake() {
            this.currentVelocity = this.Velocity;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.R)) {
                if (this.isFly) {
                    this.StopFlying();
                }
                else {
                    this.currentVelocity = this.Velocity;
                    this.sphere.position = this.Comparer.position;

                    this.radianAngle = Mathf.Deg2Rad * this.Angle;
                    this.maxDistance = (this.Velocity * this.Velocity * Mathf.Sin(2 * this.radianAngle)) / this.Gravity;

                    this.isFly = true;
                }
            }
        }

        private void FixedUpdate() {
            if (this.isFly) {
                this.time += Time.deltaTime;

                var nextHypotheticalPoint = this.CalculateNextPoint(this.time);
                var newDirection          = nextHypotheticalPoint - this.sphere.position;
                newDirection = newDirection.normalized;

                Debug.DrawRay(this.sphere.position, newDirection, Color.blue);

                if (Physics.SphereCast(this.sphere.position, this.radius, newDirection, out this.hit, newDirection.magnitude, this.layerMask)) {
                    this.StopFlying();
                }
                else {
                    this.sphere.position = nextHypotheticalPoint;
                }
            }
        }

        private void StopFlying() {
            this.time  = 0;
            this.isFly = false;
        }
    }
}