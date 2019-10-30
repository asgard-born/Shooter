namespace Managers {
    using UnityEngine;

    public class PlayerInputController : InputController {
        public static PlayerInputController Instance;

        private bool  notSerialFire;
        private float serialRate = .5f;

        private void Awake() => Instance = this;

        private void Update() {
            this.rotateX += Input.GetAxis("Mouse X");
            this.rotateY -= Input.GetAxis("Mouse Y");

            this.forwardMoving    = Input.GetAxis("Vertical");
            this.horizontalMoving = Input.GetAxis("Horizontal");
            this.isSneak          = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            this.runValue         = Input.GetButton("Run") && !this.isSneak ? 1 : 0;

            this.forwardMoving    += this.forwardMoving    > 0 ? this.runValue : this.forwardMoving < 0 ? -this.runValue : 0;
            this.horizontalMoving += this.horizontalMoving > 0 ? this.runValue : this.horizontalMoving < 0 ? -this.runValue : 0;

            this.isJumping = Input.GetKeyDown(KeyCode.Space);

            if (Input.GetMouseButtonDown(0)) {
                this.FireOnce();
            }
            //TODO fixed
            else if (Input.GetMouseButton(0) && !IsInvoking("Fire") && !this.notSerialFire) {
                Invoke(nameof(this.FireOnce), this.serialRate);
            }
            else if (Input.GetMouseButtonUp(0)) {
                this.StopFire();
                this.notSerialFire = true;
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                this.isReload = true;
            }
        }
    }
}