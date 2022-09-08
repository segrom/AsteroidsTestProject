using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class Ufo: PhysicBody
    {
        private Ship _player;
        private GameConfiguration _configuration;
        public override float Radius => 0.3f;

        public Ufo(Ship player, GameConfiguration configuration)
        {
            Position = new Vector2(Mathf.Cos(Random.value * 2f * Mathf.PI), Mathf.Sin(Random.value * 2f * Mathf.PI )).normalized * 10f;
            _player = player;
            _configuration = configuration;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Velocity = (_player.Position - Position).normalized * _configuration.ufoSpeed;
        }

        protected override void OnCollision(PhysicBody other)
        {
            if (other is not Bullet) return;
            other.Dispose();
            Dispose();
        }
    }
}