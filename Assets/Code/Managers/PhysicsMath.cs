namespace Managers {
    using UnityEngine;

    public static class PhysicsMath {
        public static Vector2 CalculateArcPoint2D(float radianAngle, float gravity, float velocity, float time, float maxDistance) {
            float x = time * maxDistance;

            float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
            return new Vector2(x, y);
        }
    }
}