using Extensions;
using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class Bullet: PhysicBody
    {
        public override float Radius => 0.05f;
        public Bullet(Vector2 position, float rotation, ShipConfiguration configuration)
        {
            Position = position;
            Rotation = rotation;
            Velocity = Rotation.RotationToVector() * configuration.bulletsSpeed;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Vector2.Distance(Vector2.zero, Position) > 10)
            {
                Dispose();
            }
        }
    }
}