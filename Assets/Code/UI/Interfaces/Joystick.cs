namespace UI.Interfaces {
    using UnityEngine;
    using UnityEngine.UI;

    public interface Joystick {
        Image  WeaponImage     { get; }
        Button ReloadBtn       { get; }
        Button ChangeWeaponBtn { get; }

        ETCJoystick MovingJoystick { get; }
        ETCJoystick GrenadeBar     { get; }
        ETCJoystick AttackJoystick { get; }

        float GetBallisticValue { get; }

        void RearrangeAttackUI(Sprite sprite, bool isThrowable);
    }
}