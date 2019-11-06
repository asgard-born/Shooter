namespace Controllers {
    using UnityEngine;

    public class CameraController : MonoBehaviour {
        public static CameraController Instance;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform lookAt;
        [SerializeField] private float     min;
        [SerializeField] private float     max;
        [SerializeField] private float     distance = 2.2f;

        private void Awake() => Instance = this;

        // Should be called in LateUpdate
        public void RotateTargetHorizontally(float mouseX) => this.lookAt.rotation = Quaternion.Euler(0, mouseX, 0);

        // Should be called in LateUpdate
        public void RotateCamera(float mouseX, float mouseY) {
            mouseY = Mathf.Clamp(mouseY, this.min, this.max);
            Vector3    offset   = new Vector3(0, 1.8f, -this.distance);
            Quaternion rotation = Quaternion.Euler(mouseY, mouseX, -this.distance);

            this.cameraTransform.position = this.lookAt.position + rotation * offset;
            this.cameraTransform.LookAt(this.lookAt.position);
        }
    }
}