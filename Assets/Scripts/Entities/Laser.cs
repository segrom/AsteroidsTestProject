using System;
using System.Threading.Tasks;
using Extensions;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class Laser: PhysicBody
    {
        public int Cooldown { get; private set; }
        private readonly Action _onDispose;
        private readonly Ship _player;
        private readonly ShipConfiguration _configuration;
        private Vector2 _endPoint;

        private bool _isWorking;
        private Task _laserLifetimeTask;

        public override float Radius => 20f;

        public Laser(Ship player, ShipConfiguration configuration, Action onDispose)
        {
            _onDispose = onDispose;
            _player = player;
            _configuration = configuration;
            Cooldown = 0;
            StartLifetime();
        }

        private async void StartLifetime()
        {
            _laserLifetimeTask = LaserLifetime();
            await _laserLifetimeTask;
            _onDispose();
        }

        public override Vector2 GetScale(){
            return new Vector2( _isWorking ? 10 : 0, 1);
        }

        public override void Update(float deltaTime)
        {
            Position = _player.Position;
            Rotation = _player.Rotation;
            _endPoint = Position + Rotation.RotationToVector() * 10f;
            base.Update(deltaTime);
        }

        protected override void OnCollision(PhysicBody other)
        {
            if(!_isWorking || other is Ship or Bullet) return;
            var distance = other.Position.DistanceToSegment(Position, _endPoint, out _);
            if ( distance < other.Radius + _configuration.laserRadius)
            {
                other.Dispose();
            }
        }

        private async Task LaserLifetime()
        {
            _isWorking = true;
            await Task.Delay((int)(_configuration.laserDuration * 1000f));
            _isWorking = false;
            Cooldown = _configuration.laserDelay;
            for (int i = Cooldown; i >= 0; i--)
            {
                await Task.Delay(1000);
                Cooldown = i;
            }
            
        }

    }
}