namespace Controllers {
    using Managers;
    using UnityEngine;

    public abstract class BallisticController : MonoBehaviour {
        public Transform Comparer;

        public float Velocity;
        public float Angle;
        public float Gravity = 30f;

        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected float     radius = .5f;

        protected RaycastHit hit;
        protected float      currentVelocity;
        protected float      radianAngle;
        protected float      maxDistance;
        protected float      time;

        protected Vector3 CalculateNextPoint(float timepoint) {
            var arcPoint2D = PhysicsMath.CalculateArcPoint2D(this.radianAngle, this.Gravity, this.currentVelocity, timepoint, this.maxDistance);
            var offset     = new Vector3(0, arcPoint2D.y, arcPoint2D.x);

            return this.Comparer.position + this.Comparer.rotation * offset;
        }
    }
}