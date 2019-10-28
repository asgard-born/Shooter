namespace Structures {
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public GameObject WeaponObject;

        public int       Id;
        public string    WeaponName;
        public int       Damage;
        public float     EquipRate;
        public float     HolsterRate;
//        public AudioClip AttackSound;
    }
}