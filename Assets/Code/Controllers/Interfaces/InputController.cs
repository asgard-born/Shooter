namespace Controllers.Interfaces {
    using System;

    public interface InputController {
        float GetAxisX         { get; }
        float GetAxisY         { get; }
        float HorizontalMoving { get; }
        float ForwardMoving    { get; }
        bool  IsSneak          { get; }
        bool  IsJumping        { get; }

        float SerialRate { set; }

        event Action OnFireOnce;
        event Action OnReload;
        event Action OnChangingWeapon;
    }
}