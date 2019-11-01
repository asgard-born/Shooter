namespace Controllers {
    using Managers;
    using UnityEngine;

    public class GrenadeController : MonoBehaviour {
        public Transform sphere;

        public Transform initialTrans;
        public float     Velocity;
        public float     Angle;
        public float     Gravity = 30f;

        private float time;
        private bool  isFly;
        private float speedFactorZ;
        private float maxDistance;
        private float radianAngle;

        private void Awake() {
            this.speedFactorZ = this.Velocity;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (this.isFly) {
                    this.time            = 0;
                    this.isFly           = false;
                    this.sphere.position = this.initialTrans.position;
                }
                else {
                    this.Angle        = Vector3.Angle(this.sphere.forward, this.initialTrans.forward);
                    this.speedFactorZ = this.Velocity;

                    this.radianAngle = Mathf.Deg2Rad * this.Angle;
                    this.maxDistance = (this.Velocity * this.Velocity * Mathf.Sin(2 * this.radianAngle)) / this.Gravity;

                    this.isFly = true;
                }
            }

            if (this.isFly) {
                this.time += Time.fixedDeltaTime;
                var newPosition = PhysicsMath.CalculateArcPoint(this.radianAngle, this.Gravity, this.Velocity, this.time, this.maxDistance);
                this.sphere.localPosition = new Vector3(this.sphere.localPosition.x, newPosition.y, newPosition.x);
            }
        }
    }
}