using Extensions;
using UnityEngine;

namespace Entities
{
    public class Asteroid: PhysicBody
    {
        public override float Radius => 0.34f;
        private ObjectsController _objectsController;
        public Asteroid(ObjectsController objectsController, float speed)
        {
            _objectsController = objectsController;
            Position = (Random.value * 2f * Mathf.PI).RotationToVector() * 10f;
            Rotation = Position.GetAngleToPoint(new Vector2(0,0));
            Velocity = Rotation.RotationToVector() * speed;
        }

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
            SpawnParts(3);
            other.Dispose();
            Dispose();
        }

        private void SpawnParts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var part = new SmallAsteroid();
                part.Position = Position;
                part.Rotation = Random.Range(0, 360f);
                part.Velocity = Velocity + part.Rotation.RotationToVector();
                _objectsController.AddUpdatable(part);
            }
        }
    }
}