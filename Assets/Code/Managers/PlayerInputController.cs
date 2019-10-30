﻿namespace Managers {
    using UnityEngine;

    public class PlayerInputController : InputController {
        public static PlayerInputController Instance;
        
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
            else if (Input.GetMouseButtonUp(0)) {
                this.StopFire();
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                this.isReload = true;
            }
        }
    }
}