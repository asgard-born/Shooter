namespace Controllers {
    using Interfaces;
    using System;
    using UnityEngine;

    public class JoystickController : MonoBehaviour, InputController {
        public float ForwardMoving    { get; }
        public float HorizontalMoving { get; }
        public float GetAxisX         { get; }
        public float GetAxisY         { get; }
        public bool  IsSneak          { get; }
        public bool  IsJumping        { get; }
        public bool  CanInputFire     { get; }
        public float SerialRate       { get; set; }

        public event Action OnFireOnce;
        public event Action OnReload;

        public void FireOnce() {
        }

        public void Reload() {
        }
    }
}