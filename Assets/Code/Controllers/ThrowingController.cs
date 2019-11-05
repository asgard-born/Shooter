namespace Controllers {
    using Structures.WeaponTypes;
    using Structures;
    using Managers;
    using UnityEngine;

    public abstract class ThrowingController : BallisticController {
        [SerializeField] private Transform comparerHolder;

        protected Transform weaponTransform;

        protected string    weaponName;
        protected float     blowingTime;
        protected int       damage, id_attacker, id_weapon;
        protected bool      isFly;
        public  LayerMask attackingLayerMask;

        protected void OnWeaponReachedTarget() {
            
        }

        protected override Vector3 CalculateNextPoint(float timepoint) {
            var arcPoint2D = PhysicsMath.CalculateArcPoint2D(this.radianAngle, this.Gravity, this.currentVelocity, timepoint, this.maxDistance);
            var offset     = new Vector3(0, arcPoint2D.y, arcPoint2D.x);

            return this.Comparer.position + this.Comparer.rotation * offset;
        }

        protected void InitializeWeapon(Transform weaponTransform, int damage, int id_attacker, int id_weapon, string weaponName, LayerMask layerMask) {
            this.weaponTransform    = weaponTransform;
            this.damage             = damage;
            this.id_attacker        = id_attacker;
            this.id_weapon          = id_weapon;
            this.weaponName         = weaponName;
            this.attackingLayerMask = layerMask;
        }

        public void StartFlyingProcess(Transform weaponTransform, int damage, int id_attacker, int id_weapon, string weaponName, LayerMask layerMask) {
            this.InitializeWeapon(weaponTransform, damage, id_attacker, id_weapon, weaponName, layerMask);

            this.weaponTransform.SetParent(null);
            this.Comparer.SetParent(null);

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
                var newDirection          = nextHypotheticalPoint - this.weaponTransform.position;
                newDirection = newDirection.normalized;

                Debug.DrawRay(this.weaponTransform.position, newDirection, Color.blue);

                if (Physics.SphereCast(this.weaponTransform.position, this.radius, newDirection, out this.hit, newDirection.magnitude * .5f, this.layerMask)) {
                    this.weaponTransform.position = this.hit.point - this.weaponTransform.forward * .3f;
                    this.StopFlying();

                    this.OnWeaponReachedTarget();
                }
                else {
                    this.weaponTransform.position = nextHypotheticalPoint;
                }
            }
        }

        private void StopFlying() {
            this.time  = 0;
            this.isFly = false;
            this.Comparer.SetParent(this.comparerHolder);
            this.Comparer.position = this.comparerHolder.position;
            this.Comparer.rotation = this.comparerHolder.rotation;
        }
    }
}