namespace Controllers {
    using System;
    using UnityEngine;

    public class ArcRenderer : MonoBehaviour {
        public LineRenderer LineRenderer;
        public Transform    sphere;

        public Transform initialTrans;
        public float     SpeedFactor;
        public float     Angle;
        public float     Gravity = 9.8f;

        [SerializeField] private int positionCount = 16;

        private float time;
        private bool  isRendering;
        private float speedFactorZ;

        public event Action<bool> OnRenderArc;

        private void Awake() {
            this.speedFactorZ = this.SpeedFactor;

            this.OnRenderArc += this.ArrangeRendering;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                this.OnRenderArc?.Invoke(true);
            }
        }

        private void ArrangeRendering(bool isRendering) {
            if (isRendering) {
                this.Angle        =  Vector3.Angle(this.sphere.forward, this.initialTrans.forward);
                this.Angle        *= Mathf.Deg2Rad;
                this.speedFactorZ =  this.SpeedFactor;

                this.isRendering = true;

                this.Render();
            }
            else {
                this.isRendering = false;
                this.time        = 0;
            }
        }

        private void Render() {
            for (int i = 0; i < this.positionCount; i++) {
                this.SetPosition(i);

                if (i == this.positionCount - 1) {
                    this.OnRenderArc?.Invoke(false);
                }
            }
        }

        private void SetPosition(int index) {
            float vz = this.speedFactorZ * this.time * Mathf.Cos(this.Angle) * Time.fixedDeltaTime;
            float vy = this.SpeedFactor * this.time * Mathf.Sin(this.Angle) * Time.fixedDeltaTime - this.Gravity * (this.time * this.time) / 2 * Time.fixedDeltaTime;

            var position = new Vector3(0, vy, vz);
            this.LineRenderer.SetPosition(index + 1, position);
            this.speedFactorZ -= .25f;
            this.speedFactorZ =  Mathf.Clamp(this.speedFactorZ, this.SpeedFactor / 4f, this.SpeedFactor);

            this.time += Time.fixedDeltaTime * this.SpeedFactor;
        }
    }
}