namespace Controllers {
    using Managers;
    using UnityEngine;

    public class ArcRenderer : MonoBehaviour {
        public Transform sphere;
        public Transform initialTrans;

        private LineRenderer lineRenderer;

        [SerializeField] private float velocity;
        [Range(.5f,50f)][SerializeField] private float fadingFactor;
        [SerializeField] private float angle;
        [SerializeField] private int   pointsCount;
        [SerializeField] private float gravity;

        private float currentVelocity;
        private float currentFadingFactor;
        private float radianAngle;

        private void Awake() {
            this.angle           = 45;
            this.currentVelocity = this.velocity;
            this.lineRenderer    = this.GetComponent<LineRenderer>();
            this.lineRenderer.SetVertexCount(this.pointsCount);
        }

        private void Render() {
            this.currentFadingFactor = this.fadingFactor / 10000;
            this.lineRenderer.SetPositions(this.CalculateArcArray());
        }

        private void Update() {
            this.Render();
        }

        private Vector3[] CalculateArcArray() {
            Vector3[] arcArray = new Vector3[this.pointsCount];

            this.radianAngle = Mathf.Deg2Rad * this.angle;
            float maxDistance = (this.currentVelocity * this.currentVelocity * Mathf.Sin(2 * this.radianAngle)) / this.gravity;

            for (int i = 0; i < this.pointsCount; i++) {
                float time    = (float) i / this.pointsCount;
                var   point2D = PhysicsMath.CalculateArcPoint(this.radianAngle, this.gravity, this.currentVelocity, time, maxDistance);
                var   offset  = new Vector3(0, point2D.y, point2D.x);

                arcArray[i]          =  this.sphere.position + this.sphere.rotation * offset;
//                this.currentVelocity -= this.currentVelocity * this.currentFadingFactor;

                this.currentVelocity = Mathf.Clamp(this.currentVelocity, 10, 9999);

                if (i + 1 == this.pointsCount) {
                    this.currentVelocity = this.velocity;
                }
            }

            return arcArray;
        }
    }
}