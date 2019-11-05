namespace Controllers {
    using System.Threading.Tasks;
    using System;
    using UnityEngine;

    public class KeyboardController : MonoBehaviour, InputController {
        public float GetAxisX         => Input.GetAxis("Mouse X");
        public float GetAxisY         => Input.GetAxis("Mouse Y");
        public float HorizontalMoving => this.horizontalMoving;
        public float ForwardMoving    => this.forwardMoving;
        public int   runValue         => Input.GetButton("Run") && !this.IsSneak ? 1 : 0;
        public bool  IsSneak          => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        public bool  IsJumping        => Input.GetKeyDown(KeyCode.Space);
        public bool  CanInputFire     => this.canInputFire;

        public float SerialRate { get; set; }

        private bool  canInputFire = true;
        private float serialRate;

        private float forwardMoving;
        private float horizontalMoving;

        public event Action OnFireOnce;
        public event Action OnReload;

        public void FireOnce() => this.OnFireOnce?.Invoke();
        public void Reload()   => this.OnReload?.Invoke();

        private void Update() {
            this.forwardMoving    = Input.GetAxis("Vertical");
            this.horizontalMoving = Input.GetAxis("Horizontal");

            this.forwardMoving    += this.forwardMoving > 0 ? this.runValue : this.forwardMoving < 0 ? -this.runValue : 0;
            this.horizontalMoving += this.horizontalMoving > 0 ? this.runValue : this.horizontalMoving < 0 ? -this.runValue : 0;

            if (Input.GetMouseButtonDown(0) && this.canInputFire) {
                PerformFireInputWithFireRate();
            }
            else if (Input.GetMouseButton(0) && this.canInputFire) {
                PerformFireInputWithFireRate();
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                this.Reload();
            }

            async void PerformFireInputWithFireRate() {
                this.canInputFire = false;
                this.FireOnce();
                await Task.Delay(TimeSpan.FromSeconds(this.SerialRate));
                this.canInputFire = true;
            }
        }
    }
}