namespace Abilities {
    using System;

    [Serializable]
    public struct Buff {
        public StatType         StatType;
        public StatModifierType ModifierType;
        public float            Value;
    }
}