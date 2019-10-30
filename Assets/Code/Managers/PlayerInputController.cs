﻿using System;
using System.Threading.Tasks;

namespace Managers {
    using UnityEngine;

    public class PlayerInputController : InputController {
        public static PlayerInputController Instance;

        private bool  canInputFire = true;
        private float serialRate = .04f;

        private void Awake() => Instance = this;

        private async void Update() {
            this.rotateX += Input.GetAxis("Mouse X");
            this.rotateY -= Input.GetAxis("Mouse Y");

            this.forwardMoving    = Input.GetAxis("Vertical");
            this.horizontalMoving = Input.GetAxis("Horizontal");
            this.isSneak          = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            this.runValue         = Input.GetButton("Run") && !this.isSneak ? 1 : 0;

            this.forwardMoving    += this.forwardMoving    > 0 ? this.runValue : this.forwardMoving < 0 ? -this.runValue : 0;
            this.horizontalMoving += this.horizontalMoving > 0 ? this.runValue : this.horizontalMoving < 0 ? -this.runValue : 0;

            this.isJumping = Input.GetKeyDown(KeyCode.Space);

            if (Input.GetMouseButtonDown(0) && this.canInputFire) {
                PerformFireInputWithFireRate();
            }
            //TODO change logic
            else if (Input.GetMouseButton(0) && this.canInputFire) {
                PerformFireInputWithFireRate();
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                this.isReload = true;
            }

            async void PerformFireInputWithFireRate() {
                this.canInputFire = false;
                this.FireOnce();
                await Task.Delay(TimeSpan.FromSeconds(this.serialRate));
                this.canInputFire = true;
            }
        }
    }
}