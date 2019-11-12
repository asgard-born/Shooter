using System.Collections.Generic;

namespace Abilities {
    public class Ability {
        public AbilityType AbilityType;
        public AbilityTarget AbilityTarget;
        public List<Buff> Buffs = new List<Buff>(16);
    }
}