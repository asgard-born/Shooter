namespace Abilities.Interfaces {
    using System;

    public interface IActivableBuffable : IBuffable {
        bool IsActive { get; set; }

        event Action OnActivated;
        event Action OnInactivated;
    }
}