using UnityEngine;

namespace Controllers {
    public class CameraController : MonoBehaviour {
        private float mouseX;
        private float mouseY;

        [SerializeField] private Transform lookAt;
        [SerializeField] private float     min;
        [SerializeField] private float     max;
        [SerializeField] private float     distance = 2.2f;

        private void Update() {
            Cursor.visible =  false;
            this.mouseX    += Input.GetAxis("Mouse X");
            this.mouseY    -= Input.GetAxis("Mouse Y");
            this.mouseY    =  Mathf.Clamp(this.mouseY, this.min, this.max);

            this.lookAt.rotation = Quaternion.Euler(0, this.mouseX, 0);
        }

        private void LateUpdate() {
            Vector3    offset   = new Vector3(0, 1.8f, -this.distance);
            Quaternion rotation = Quaternion.Euler(this.mouseY, this.mouseX, -this.distance);

            this.transform.position = this.lookAt.position + rotation * offset;
            this.transform.LookAt(this.lookAt.position);
        }
    }
}