namespace Structures {
    using UnityEngine;

    public class Bullet : MonoBehaviour {
        private Vector3 startingPoint;
        private float   weaponRange;
        private float   travelDistance;
        private int     damage;
        private int     bulletSpeed;
        private bool    isBulletFly;

        private RaycastHit raycastHit;

        public void Initialize(Vector3 startingPoint, float weaponRange, float travelDistance, int damage, int bulletSpeed, bool isBulletFly) {
            this.startingPoint  = startingPoint;
            this.weaponRange    = weaponRange;
            this.travelDistance = travelDistance;
            this.damage         = damage;
            this.bulletSpeed    = bulletSpeed;
            this.isBulletFly    = isBulletFly;
        }

        public void Fly(float weaponRange, Vector3 startingPoint) {
            this.travelDistance += Time.deltaTime * this.bulletSpeed;
            Debug.DrawRay(startingPoint, this.transform.forward * this.travelDistance, Color.cyan);

            if (Physics.Raycast(startingPoint, this.transform.forward, out this.raycastHit, this.travelDistance)) {
                this.isBulletFly = false;
                this.gameObject.SetActive(false);

                var life = this.raycastHit.transform.gameObject.GetComponent<Life>();

                if (life != null) {
                    life.GetDamage(this.damage);
                    this.travelDistance = 0;
                }
            }

            if (this.travelDistance >= weaponRange) {
                this.gameObject.SetActive(false);
                this.isBulletFly    = false;
                this.travelDistance = 0;
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