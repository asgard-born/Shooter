namespace Managers {
    using System;
    using UnityEngine;

    public abstract class InputController : MonoBehaviour {
        public float ForwardMoving    => this.forwardMoving;
        public float HorizontalMoving => this.horizontalMoving;
        public float RotateX          => this.rotateX;
        public float RotateY          => this.rotateY;
        public int   RunValue         => this.runValue;
        public bool  IsSneak          => this.isSneak;
        public bool  IsJumping        => this.isJumping;

        public event Action OnFireOnce;
        public event Action OnReload;

        protected float rotateX;
        protected float rotateY;

        protected float forwardMoving;
        protected float horizontalMoving;
        protected int   runValue;
        protected bool  isSneak;
        protected bool  isJumping;

        protected void FireOnce() => this.OnFireOnce?.Invoke();
        protected void Reload()   => this.OnReload?.Invoke();
    }
}