using ObjectPool;

namespace Structures {
    using UnityEngine;

    public class Bullet : MonoBehaviour {
        private Vector3   startingPoint;
        private float     weaponRange;
        private float     travelledDistance;
        private int       damage;
        private int       bulletSpeed;
        private int       id_attacker;
        private int       id_weapon;
        private string    weaponName;
        private bool      isBulletFly;
        private LayerMask layerMask;

        private RaycastHit raycastHit;

        public void Initialize(Vector3 startingPoint,
                               float weaponRange,
                               float travelledDistance,
                               int damage,
                               int bulletSpeed,
                               int id_attacker,
                               int id_weapon,
                               string weaponName,
                               bool isBulletFly,
                               LayerMask layerMask) {
            this.startingPoint     = startingPoint;
            this.weaponRange       = weaponRange;
            this.travelledDistance = travelledDistance;
            this.damage            = damage;
            this.bulletSpeed       = bulletSpeed;
            this.id_attacker       = id_attacker;
            this.id_weapon         = id_weapon;
            this.weaponName        = weaponName;
            this.isBulletFly       = isBulletFly;
            this.layerMask         = layerMask;
        }

        public void Fly(float weaponRange, Vector3 startingPoint) {
            this.travelledDistance += Time.deltaTime * this.bulletSpeed;
            Debug.DrawRay(startingPoint, this.transform.forward * this.travelledDistance, Color.cyan);

            if (Physics.Raycast(startingPoint, this.transform.forward, out this.raycastHit, this.travelledDistance, layerMask)) {
                this.isBulletFly = false;
                var lifer = this.raycastHit.transform.gameObject.GetComponent<Lifer>();

                if (lifer != null) {
                    lifer.Hit(this.damage, this.id_attacker, this.id_weapon, this.weaponName);
                    this.travelledDistance = 0;
                }

                this.GetComponent<PoolObject>().ReturnToPool();
            }

            if (this.travelledDistance >= weaponRange) {
                this.GetComponent<PoolObject>().ReturnToPool();
                this.isBulletFly       = false;
                this.travelledDistance = 0;
            }

            this.transform.position += this.transform.forward * Time.deltaTime * this.bulletSpeed;
        }

        private void Update() {
            if (this.isBulletFly) {
                this.Fly(this.weaponRange, this.startingPoint);
            }
        }
    }
}