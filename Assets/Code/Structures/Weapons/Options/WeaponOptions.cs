namespace Structures.Weapons.Options {
    using System;

    [Serializable]
    public class WeaponOptions {
        public int    id;
        public string weaponName;
        public float  serialRate;
        public int    damage;
        public int    attackSpeed;
        public int    range;
        public int    magazineCapacity;
        public float  reloadRate;
        public float  blowingTime;
    }
}