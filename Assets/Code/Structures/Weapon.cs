namespace Structures {
    using System;
    using System.Threading.Tasks;
    using Managers;
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public GameObject WeaponObject;

        public int    Id;
        public string WeaponName;
        public int    Damage;
        public int    Ammo;
        public int    Speed;
        public float  Range;

        public float SerialRate;
        public float ReloadRate;

        [SerializeField] protected Transform aim;

        protected Vector3 fireDirection;

        protected PoolManager poolManager;

        private async void Awake() {
            this.poolManager = PoolManager.Instance;


            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }
        }

        public void Fire() {
            if (this.Ammo > 0) {
                this.Ammo--;

                RaycastHit hit;

                // if there is some object on the weapon range distance - it became a target and rotation point
                if (Physics.Raycast(this.aim.position, this.aim.forward, out hit, this.Range)) {
                    this.fireDirection = (hit.point - this.aim.position).normalized;
                    // otherwise take target and rotation point from max possibility of current weapon range
                }
                else {
                    Vector3 endPoint = this.aim.position + this.aim.forward * this.Range;

                    this.fireDirection = (endPoint - this.aim.position).normalized;
                }

                // Get Bullet from pool
                var bullet = this.poolManager
                                 .GetObject("Bullet", this.aim.position, Quaternion.LookRotation(this.fireDirection))
                                 .GetComponent<Bullet>();

                bullet.Initialize(bullet.transform.position,
                    this.Range,
                    0,
                    this.Damage,
                    this.Speed,
                    true);
            }
        }
    }
}