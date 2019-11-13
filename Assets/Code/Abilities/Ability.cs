namespace Abilities {
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Ability {
        public AbilityType AbilityType;
        public List<Buff>  Buffs = new List<Buff>(16);
    }
}