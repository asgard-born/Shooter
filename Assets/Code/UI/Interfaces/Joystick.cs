namespace UI.Interfaces {
    using UnityEngine;
    using UnityEngine.UI;

    public interface Joystick {
        AttackButton AttackBtn       { get; }
        Button ReloadBtn       { get; }
        Button ChangeWeaponBtn { get; }

        ETCJoystick MovingJoystick { get; }
        ETCJoystick GrenadeBar     { get; }

        float GetBallisticValue { get; }

        void RearrangeAttackUI(Sprite sprite, bool isThrowable);
    }
}