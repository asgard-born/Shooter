namespace Controllers {
    using UnityEngine;

    public class GrenadeFlowController : MonoBehaviour {
        public Transform sphere;

        public Transform initialTrans;
        public float     SpeedFactor;
        public float     Angle;
        public float     Gravity = 9.8f;

        
        private float        time;
        private bool         isFly;
        private float        speedFactorZ;

        private void Awake() {
            this.speedFactorZ = this.SpeedFactor;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (this.isFly) {
                    this.time            = 0;
                    this.isFly           = false;
                    this.sphere.position = this.initialTrans.position;
                }
                else {
                    this.Angle        =  Vector3.Angle(this.sphere.forward, this.initialTrans.forward);
                    this.Angle        *= Mathf.Deg2Rad;
                    this.speedFactorZ =  this.SpeedFactor;

                    this.isFly = true;
                }
            }

            if (this.isFly) {
                this.time += Time.deltaTime;

                Debug.Log(this.speedFactorZ);

                float vz = this.speedFactorZ * this.time * Mathf.Cos(this.Angle) * Time.deltaTime;
                float vy = this.SpeedFactor * this.time * Mathf.Sin(this.Angle) * Time.deltaTime - this.Gravity * (this.time * this.time) / 2 * Time.deltaTime;

                this.sphere.localPosition += new Vector3(0, vy, vz);
                this.speedFactorZ         -= .25f;
                this.speedFactorZ         =  Mathf.Clamp(this.speedFactorZ, this.SpeedFactor / 4f, this.SpeedFactor);
            }
        }
    }
}