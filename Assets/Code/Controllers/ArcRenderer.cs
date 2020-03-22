namespace Controllers {
    using System;
    using Managers;
    using UnityEngine;

    public class ArcRenderer : BallisticController {
        public int PointsCount;

        private LineRenderer lineRenderer;
        private Vector3[]    arcArray;

        private int currentIteration;

        public void SetupRendering(bool isOn)
            => this.lineRenderer.enabled = isOn;

        protected override Vector3 CalculateNextPoint(float timepoint) {
            var arcPoint2D = PhysicsMath.CalculateArcPoint2D(this.radianAngle, this.Gravity, this.currentVelocity, timepoint, this.maxDistance);
            var offset     = new Vector3(0, arcPoint2D.y, arcPoint2D.x);

            return this.Comparer.position + this.Comparer.rotation * offset;
        }

        private void Awake() {
            this.arcArray = new Vector3[this.PointsCount];

            this.lineRenderer = this.GetComponent<LineRenderer>();
            this.lineRenderer.SetVertexCount(this.PointsCount);
        }

        private void Render() {
            this.lineRenderer.SetPositions(this.CalculateArcArray());
        }

        private void FixedUpdate() {
            this.Render();
            this.currentVelocity = this.Velocity;
        }

        private Vector3[] CalculateArcArray() {
            Array.Clear(this.arcArray, 0, this.arcArray.Length);

            this.radianAngle = Mathf.Deg2Rad * this.Angle;
            this.maxDistance = (this.currentVelocity * this.currentVelocity * Mathf.Sin(2 * this.radianAngle)) / this.Gravity;

            bool isCollisionDetected = false;

            for (int i = 0; i < this.PointsCount; i++) {
                this.currentIteration = i;

                if (isCollisionDetected) {
                    this.arcArray[this.currentIteration] = this.arcArray[this.currentIteration - 1];
                }
                else if (!this.CalculateIteration()) {
                    isCollisionDetected = true;
                }
            }

            return this.arcArray;
        }

        private Vector3 CheckForCollision() {
            Vector3 nextHypotheticalPoint = this.CalculateNextPoint(this.time);

            var direction = nextHypotheticalPoint - this.arcArray[this.currentIteration - 1];

            Debug.DrawRay(this.arcArray[this.currentIteration - 1], direction, Color.blue);


            if (Physics.SphereCast(this.arcArray[this.currentIteration - 1], this.radius, direction.normalized, out this.hit, direction.magnitude, this.layerMask)) {
                return this.hit.point;
            }

            return Vector3.zero;
        }

        private bool CalculateIteration() {
            this.time = (float) this.currentIteration / this.PointsCount;

            var potentialCollision = Vector3.zero;

            if (this.currentIteration > 0) {
                potentialCollision = this.CheckForCollision();
            }

            if (potentialCollision != Vector3.zero) {
                this.arcArray[this.currentIteration] = potentialCollision;

                return false;
            }

            this.arcArray[this.currentIteration] = this.CalculateNextPoint(this.time);

            if (this.currentIteration + 1 == this.PointsCount) {
                this.currentVelocity = this.Velocity;
            }

            return true;
        }
    }
}