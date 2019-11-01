namespace Controllers {
    using UnityEngine;

    public class ArcRendererAlt : MonoBehaviour {
        private LineRenderer lineRenderer;

        [SerializeField] private float velocity;
        [SerializeField] private float angle;
        [SerializeField] private int   pointsCount;

        [SerializeField] private float gravity;
        private float radianAngle;

        private void Awake() {
            this.lineRenderer = this.GetComponent<LineRenderer>();
//            this.gravity      = Mathf.Abs(Physics.gravity.y);
        }

        private void Start() {
            this.Render();
        }

        private void Render() {
            this.lineRenderer.SetVertexCount(this.pointsCount);
            this.lineRenderer.SetPositions(this.CalculateArcArray());
        }

        private Vector3[] CalculateArcArray() {
            Vector3[] arcArray = new Vector3[this.pointsCount];

            this.radianAngle = Mathf.Deg2Rad * this.angle;
            float maxDistance = (this.velocity * this.velocity * Mathf.Sin(2 * this.radianAngle)) / this.gravity;

            for (int i = 0; i < this.pointsCount; i++) {
                float time = (float) i / (float) this.pointsCount;
                arcArray[i] = this.CalculateArcPoint(time, maxDistance);
            }

            return arcArray;
        }

        private Vector3 CalculateArcPoint(float time, float maxDistance) {
            float z = time * maxDistance;
            float y = z * Mathf.Tan(this.radianAngle) - ((this.gravity * z * z) / (2 * this.velocity * this.velocity * Mathf.Cos(this.radianAngle) * Mathf.Cos(this.radianAngle)));
            return new Vector3(0, y, z);
        }
    }
}