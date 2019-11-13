namespace Abilities {
    using System.Collections.Generic;

    public class Ability {
        public AbilityType AbilityType;
        public List<Buff>  Buffs = new List<Buff>(16);
    }
}