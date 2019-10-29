using System;
using System.Threading.Tasks;

namespace Structures {
    using Managers;
    using UI;
    using UnityEngine;

    public abstract class Weapon : MonoBehaviour {
        public GameObject WeaponObject;

        public int    Id;
        public string WeaponName;
        public int    Damage;
        public int    Ammo;
        public float  Range;

        public float SerialRate;
        public float ReloadRate;

        public Bullet bullet;

        [SerializeField] private Transform aim;

        private Vector3 fireDirection;

        private Crosshair   crosshair;
        private PoolManager poolManager;

        private async void Awake() {
            this.crosshair   = Crosshair.Instance;
            this.poolManager = PoolManager.Instance;

            while (this.crosshair == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.crosshair = Crosshair.Instance;
            }

            while (this.poolManager == null) {
                await Task.Delay(TimeSpan.FromSeconds(.1f));
                this.poolManager = PoolManager.Instance;
            }
        }

        public void Fire() {
            if (this.Ammo > 0) {
                this.Ammo--;

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(
                    Screen.width / 2,
                    Screen.height / 2 + this.crosshair.CrosshairRegulator
                ));

                Vector3 startingPoint = ray.origin;

                RaycastHit hit;

                // if there is some object on the weapon range distance - it became a target and rotation point
                if (Physics.Raycast(startingPoint, ray.direction, out hit, this.Range)) {
                    this.fireDirection = (hit.point - this.aim.position).normalized;
                    // otherwise take target and rotation point from max possibility of current weapon range
                }
                else {
                    Vector3 endPoint = startingPoint + ray.direction * this.Range;
                    this.fireDirection = (endPoint   - this.aim.position).normalized;
                }

                // Get Bullet from pool
                this.bullet = this.poolManager.GetObject("Bullet", this.aim.position, Quaternion.LookRotation(this.fireDirection)).GetComponent<Bullet>();
            }
        }
    }
}