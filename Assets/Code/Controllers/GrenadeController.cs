namespace Controllers {
    using Structures;
    using Managers;
    using UnityEngine;

    public class GrenadeController : BallisticController {
        [SerializeField] private Transform grenade;
        [SerializeField] private Transform ComparerHolder;

        private float blowingTime;
        private bool  isFly;

        protected override Vector3 CalculateNextPoint(float timepoint) {
            var arcPoint2D = PhysicsMath.CalculateArcPoint2D(this.radianAngle, this.Gravity, this.currentVelocity, timepoint, this.maxDistance);
            var offset     = new Vector3(0, arcPoint2D.y, arcPoint2D.x);

            return this.Comparer.position + this.Comparer.rotation * offset;
        }

        public void StartFlyingProcess(Transform grenade, int id_attacker, LayerMask layerMask) {
            this.grenade = grenade;
            this.grenade.SetParent(null);
            this.Comparer.SetParent(null);
            this.layerMask = layerMask;

            this.currentVelocity = this.Velocity;

            this.radianAngle = Mathf.Deg2Rad * this.Angle;
            this.maxDistance = (this.Velocity * this.Velocity * Mathf.Sin(2 * this.radianAngle)) / this.Gravity;

            this.isFly = true;
        }

        private void Awake() => this.currentVelocity = this.Velocity;

        private void FixedUpdate() {
            if (this.isFly) {
                this.time += Time.deltaTime;

                var nextHypotheticalPoint = this.CalculateNextPoint(this.time);
                var newDirection          = nextHypotheticalPoint - this.grenade.position;
                newDirection = newDirection.normalized;

                Debug.DrawRay(this.grenade.position, newDirection, Color.blue);

                if (Physics.SphereCast(this.grenade.position, this.radius, newDirection, out this.hit, newDirection.magnitude * .5f, this.layerMask)) {
                    this.grenade.position = this.hit.point;
                    this.StopFlying();

                    var lifer = this.hit.transform.gameObject.GetComponent<Lifer>();

                    if (lifer != null) {
                        Debug.Log("damaged");
//                        lifer.Hit(this.damage, this.id_attacker, this.id_weapon, this.weaponName);
                    }
                }
                else {
                    this.grenade.position = nextHypotheticalPoint;
                }
            }
        }

        private void StopFlying() {
            this.time  = 0;
            this.isFly = false;
            this.Comparer.SetParent(this.ComparerHolder);
            this.Comparer.position = this.ComparerHolder.position;
            this.Comparer.rotation = this.ComparerHolder.rotation;
        }
    }
}