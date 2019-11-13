using System;

namespace Controllers.Abstract {
    public interface ICommandController {
        float ForwardMoving    { get; }
        float HorizontalMoving { get; }
        float RotationX          { get; }
        float RotationY          { get; }
        float SerialRate       { set; }
        bool  IsSneak          { get; }
        bool  IsJumping        { get; }

        event Action OnFireOnce;
        event Action OnReload;
        event Action OnChangingWeapon;
    }
}