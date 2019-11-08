namespace Controllers.Interfaces {
    using System;

    public interface CommandController {
        float ForwardMoving    { get; }
        float HorizontalMoving { get; }
        float RotateX          { get; }
        float RotateY          { get; }
        float SerialRate       { set; }
        bool  IsSneak          { get; }
        bool  IsJumping        { get; }

        event Action OnFireOnce;
        event Action OnReload;
        event Action OnChangingWeapon;
    }
}