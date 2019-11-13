using Abilities.Abstract;

namespace Managers {
    using System.Collections.Generic;
    using Abilities;
    using UnityEngine;

    public class StatManager : MonoBehaviour, IBuffable {
        public Dictionary<AbilityType, Ability> Abilities => this.abilities;

        private readonly Dictionary<AbilityType, Ability> abilities = new Dictionary<AbilityType, Ability>();

        public float MovingSpeedFactorMultiplicative = 1;
        public float MovingSpeedFactorAdditive;

        public float SerialRateFactorMultiplicative = 1;
        public float SerialRateFactorAdditive;

        public float AttackDamageFactorMultiplicative = 1;
        public float AttackDamageFactorAdditive;

        public float IncomingDamageFactorMultiplicative = 1;
        public float IncomingDamageFactorAdditive;

        public float FiringSplashFactorMultiplicative = 1;
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

            float Calculate(float multiplicativeFactor, float additiveFactor) {
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
                            if (buff.ModifierType == StatModifierType.Additive) {
                                this.MovingSpeedFactorAdditive += buff.Value;
                            }
                            else if (buff.ModifierType == StatModifierType.Multiplier) {
                                this.MovingSpeedFactorMultiplicative = this.MovingSpeedFactorMultiplicative <= 1
                                    ? buff.Value
                                    : this.MovingSpeedFactorMultiplicative + buff.Value;
                            }

                            break;

                        case StatType.IncomingDamage:
                            if (buff.ModifierType == StatModifierType.Additive) {
                                this.IncomingDamageFactorAdditive += buff.Value;
                            }
                            else if (buff.ModifierType == StatModifierType.Multiplier) {
                                this.IncomingDamageFactorMultiplicative = this.IncomingDamageFactorMultiplicative <= 1
                                    ? buff.Value
                                    : this.IncomingDamageFactorMultiplicative + buff.Value;
                            }

                            break;

                        case StatType.AttackDamage:
                            if (buff.ModifierType == StatModifierType.Additive) {
                                this.AttackDamageFactorAdditive += buff.Value;
                            }
                            else if (buff.ModifierType == StatModifierType.Multiplier) {
                                this.AttackDamageFactorMultiplicative = this.AttackDamageFactorMultiplicative <= 1
                                    ? buff.Value
                                    : this.AttackDamageFactorMultiplicative + buff.Value;
                            }
                            
                            break;

                        case StatType.SerialRate:
                            if (buff.ModifierType == StatModifierType.Additive) {
                                this.SerialRateFactorAdditive += buff.Value;
                            }
                            else if (buff.ModifierType == StatModifierType.Multiplier) {
                                this.SerialRateFactorMultiplicative = this.SerialRateFactorMultiplicative <= 1
                                    ? buff.Value
                                    : this.SerialRateFactorMultiplicative + buff.Value;
                            }
                            break;

                        case StatType.FiringSplash:
                            if (buff.ModifierType == StatModifierType.Additive) {
                                this.FiringSplashFactorAdditive += buff.Value;
                            }
                            else if (buff.ModifierType == StatModifierType.Multiplier) {
                                this.FiringSplashFactorMultiplicative = this.FiringSplashFactorMultiplicative <= 1
                                    ? buff.Value
                                    : this.FiringSplashFactorMultiplicative + buff.Value;
                            }
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