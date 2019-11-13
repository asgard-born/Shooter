namespace Controllers.Abstract {
    using UnityEngine;

    public abstract class MovementController : MonoBehaviour {
        [HideInInspector] public bool IsJumping;
        [HideInInspector] public bool IsJumpPressed;

        public float ForwardMoving;
        public float HorizontalMoving;
        public float RotationX;

        public abstract void Move();
        public abstract void Stop();

        public abstract void RotateTheCharacter();
        public abstract bool IsFalling();
        public abstract bool IsGrounded();
        public abstract bool IsNearToGround();
        public abstract void SetAimIK(bool value);
    }
}