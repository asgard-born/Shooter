namespace Managers {
    using System.Collections.Generic;
    using Abilities;
    using UnityEngine;

    public class StatManager : MonoBehaviour {
        public Dictionary<AbilityType, Ability> Abilities => this.abilities;

        private readonly Dictionary<AbilityType, Ability> abilities = new Dictionary<AbilityType, Ability>();

        public int   MovingSpeedFactorMultiplicative = 1;
        public float MovingSpeedFactorAdditive;

        public int   SerialRateFactorMultiplicative = 1;
        public float SerialRateFactorAdditive;

        public int   AttackDamageFactorMultiplicative = 1;
        public float AttackDamageFactorAdditive;

        public int   IncomingDamageFactorMultiplicative = 1;
        public float IncomingDamageFactorAdditive;

        public int   FiringSplashFactorMultiplicative = 1;
        public float FiringSplashFactorAdditive;

        public float CalculateValue(StatType statType, float value) {
            switch (statType) {
                case StatType.MovingSpeed:
                    return Calculate(this.MovingSpeedFactorMultiplicative, this.MovingSpeedFactorAdditive);

                case StatType.SerialRate:
                    return Calculate(this.SerialRateFactorMultiplicative, this.SerialRateFactorAdditive);

                case StatType.AttackDamage:
                    return Calculate(this.AttackDamageFactorMultiplicative, this.AttackDamageFactorAdditive);

                case StatType.IncomingDamage:
                    return Calculate(this.IncomingDamageFactorMultiplicative, this.IncomingDamageFactorAdditive);

                case StatType.FiringSplash:
                    return Calculate(this.FiringSplashFactorMultiplicative, this.FiringSplashFactorAdditive);

                default:
                    return Calculate(1, 0);
            }

            float Calculate(int multiplicativeFactor, float additiveFactor) {
                if (multiplicativeFactor == 0) {
                    multiplicativeFactor = 1;
                }

                var multiplicativeValue = multiplicativeFactor > 1 ? value * multiplicativeFactor : value / multiplicativeFactor;
                var additiveValue       = value * additiveFactor;

                return multiplicativeValue + additiveValue;
            }
        }

        public void AddAbility(Ability ability) {
            if (!this.Abilities.ContainsKey(ability.AbilityType)) {
                this.abilities.Add(ability.AbilityType, ability);
                this.EnableAbility(ability);
            }
        }

        public void RemoveAbility(Ability ability) {
            if (this.Abilities.ContainsKey(ability.AbilityType)) {
                this.abilities.Remove(ability.AbilityType);
                this.DisableAbility(ability);
            }
        }

        public void EnableAbility(Ability ability) {
            if (this.Abilities.ContainsKey(ability.AbilityType)) {
                foreach (var buff in ability.Buffs) {
                    switch (buff.StatType) {
                        case StatType.MovingSpeed:
//                            this.movementController.
                            break;

                        case StatType.IncomingDamage:
//                            this.lifer

                            break;

                        case StatType.AttackDamage:

                            break;

                        case StatType.SerialRate:
                            this.abilities.Add(ability.AbilityType, ability);
                            break;

                        case StatType.FiringSplash:
                            this.abilities.Add(ability.AbilityType, ability);
                            break;
                    }
                }
            }
            else {
                Debug.LogError($"{this.GetType().Name} doesn't have such ability: {ability.AbilityType.ToString()}");
            }
        }

        public void DisableAbility(Ability ability) {
            if (this.Abilities.ContainsKey(ability.AbilityType)) {
            }
        }
    }
}