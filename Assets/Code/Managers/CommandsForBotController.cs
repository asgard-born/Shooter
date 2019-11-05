namespace Managers {
    using System;
    using Controllers;

    public class CommandsForBotController : CommandController {
        private InputController inputController;
        
        public float        ForwardMoving    { get; }
        public float        HorizontalMoving { get; }
        public float        RotateX          { get; }
        public float        RotateY          { get; }
        public bool         IsSneak          { get; }
        public bool         IsJumping        { get; }
        public event Action OnFireOnce;
        public event Action OnReload;

        public float SerialRate {
            set => this.inputController.SerialRate = value;
        }

        public void Initialize(InputController inputController) {
        }
    }
}