using System.Collections.Generic;

namespace Abilities.Abstract {
    public interface IBuffable {
        Dictionary<AbilityType, Ability> Abilities { get; }

        void AddAbility(Ability ability);
        void RemoveAbility(Ability ability);
        
        void EnableAbility(Ability ability);
        void DisableAbility(Ability ability);
    }
}