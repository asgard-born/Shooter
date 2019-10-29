using System;

namespace Managers {
    using UnityEngine;

    public class InputController : MonoBehaviour {
        public static InputController Instance;

        public float ForwardMoving    => this.forwardMoving;
        public float HorizontalMoving => this.horizontalMoving;
        public float MouseX           => this.mouseX;
        public float MouseY           => this.mouseY;
        public int   RunValue         => this.runValue;
        public bool  IsSneak          => this.isSneak;
        public bool  IsJumpPressed    => this.isJumpPressed;

        public event Action OnMouseButtonDown;
        public event Action OnMouseButtonUp;

        private float mouseX;
        private float mouseY;

        // basic motion variables
        private float forwardMoving;
        private float horizontalMoving;
        private int   runValue;
        private bool  isSneak;
        private bool  isJumpPressed;

        private void Awake() => Instance = this;

        private void Update() {
            this.mouseX += Input.GetAxis("Mouse X");
            this.mouseY -= Input.GetAxis("Mouse Y");

            this.forwardMoving    = Input.GetAxis("Vertical");
            this.horizontalMoving = Input.GetAxis("Horizontal");
            this.isSneak          = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            this.runValue         = Input.GetButton("Run") && !this.isSneak ? 1 : 0;

            this.forwardMoving    += this.forwardMoving    > 0 ? this.runValue : this.forwardMoving < 0 ? -this.runValue : 0;
            this.horizontalMoving += this.horizontalMoving > 0 ? this.runValue : this.horizontalMoving < 0 ? -this.runValue : 0;

            this.isJumpPressed = Input.GetKeyDown(KeyCode.Space);

            if (Input.GetMouseButtonDown(0)) {
                this.OnMouseButtonDown?.Invoke();
            }
            else if (Input.GetMouseButtonUp(0)) {
                this.OnMouseButtonUp?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.R)) {
            }
        }
    }
}