namespace Controllers {
    using UnityEngine;

    public class ArcRenderer : BallisticController {
        public int PointsCount;

        private LineRenderer lineRenderer;
        private Vector3[]    arcArray;

        private int currentIteration;

        private void Awake() {
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
            this.arcArray = new Vector3[this.PointsCount];

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
            var nextIterationTimepoint = (float) (this.currentIteration + 1) / this.PointsCount;
            var newHypotheticalPoint   = this.CalculateNextPoint(nextIterationTimepoint);

            var direction = newHypotheticalPoint - this.arcArray[this.currentIteration];

            if (Physics.SphereCast(this.arcArray[this.currentIteration], this.radius, direction.normalized, out this.hit, direction.magnitude, this.layerMask)) {
                return this.hit.point;
            }

            return Vector3.zero;
        }

        private bool CalculateIteration() {
            this.time = (float) this.currentIteration / this.PointsCount;

            var potentialCollizion = Vector3.zero;

            if (this.currentIteration + 1 < this.PointsCount)
                potentialCollizion = this.CheckForCollision();

            if (this.currentIteration + 1 < this.PointsCount && potentialCollizion != Vector3.zero) {
                this.arcArray[this.currentIteration] = potentialCollizion;

                return false;
            }
            else {
                this.arcArray[this.currentIteration] = this.CalculateNextPoint(this.time);

                if (this.currentIteration + 1 == this.PointsCount) {
                    this.currentVelocity = this.Velocity;
                }

                return true;
            }
        }
    }
}