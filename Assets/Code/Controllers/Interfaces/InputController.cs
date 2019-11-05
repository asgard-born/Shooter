using System;

namespace Controllers.Interfaces {
    public interface InputController {
        float GetAxisX         { get; }
        float GetAxisY         { get; }
        float HorizontalMoving { get; }
        float ForwardMoving    { get; }
        bool  IsSneak          { get; }
        bool  IsJumping        { get; }
        bool  CanInputFire     { get; }

        float SerialRate { get; set; }

        event Action OnFireOnce;
        event Action OnReload;
        void         FireOnce();
        void         Reload();
    }
}