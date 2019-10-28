namespace Structures {
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public int        Id;
        public string     weaponName;
        public GameObject weaponType;
        public int        damage;
        public float      equipRate;
        public float      holsterRate;
        public AudioClip  attackSound;
    }
}