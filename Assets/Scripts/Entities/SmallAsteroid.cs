using UnityEngine;

namespace Entities
{
    public class SmallAsteroid: PhysicBody
    {
        public override float Radius => 0.14f;
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Vector2.Distance(Vector2.zero, Position) > 15)
            {
                Dispose();
            }
        }

        protected override void OnCollision(PhysicBody other)
        {
            if (other is not Bullet) return;
            other.Dispose();
            Dispose();
        }
    }
}