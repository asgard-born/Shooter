namespace Controllers {
    using Interfaces;
    using System;
    using UnityEngine;

    public class JoystickController : MonoBehaviour, InputController {
        public float ForwardMoving    => this.etcJoystick.GetAxisY();
        public float HorizontalMoving => this.etcJoystick.GetAxisX();
        public float GetAxisX         => this.etcJoystick.GetAxisX();
        public float GetAxisY         => this.etcJoystick.GetAxisY();
        public bool  IsSneak          { get; }
        public bool  IsJumping        { get; }
        public float SerialRate       { get; set; }

        public event Action OnFireOnce;
        public event Action OnReload;

        [SerializeField] private ETCJoystick etcJoystick;

        public void FireOnce() {
        }

        public void Reload() {
        }

        private void Update() {
        }
    }
}