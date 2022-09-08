using System.Threading.Tasks;
using Extensions;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public class Ship: PhysicBody
    {
        public int LaserCount { get; private set; }
        public Laser Laser { get; set; }
        public override float Radius => 0.3f;

        private readonly ShipConfiguration _configuration;
        private readonly ObjectsController _objectsController;
        private readonly MainInput _input;

        private readonly Vector2 _bottomLeftScreenPoint;
        private readonly Vector2 _topRightScreenPoint;

        public Ship(ShipConfiguration configuration, ObjectsController objectsController)
        {
            _configuration = configuration;
            _objectsController = objectsController;
            _input = new MainInput();
            _input.Enable();
            _input.Ship.Fire.performed += Fire;
            _input.Ship.LaserShot.performed += LaserShot;

            LaserCount = _configuration.laserCount;

            _bottomLeftScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
            _topRightScreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        }

        public override void Dispose()
        {
            _input.Disable();
            _input.Dispose();
            base.Dispose();
        }

        private void Fire(InputAction.CallbackContext obj)
        {
            var bullet = new Bullet(Position, Rotation, _configuration);
            _objectsController.AddUpdatable(bullet);
        }
        
        private void LaserShot(InputAction.CallbackContext obj)
        {
            if(Laser is not null || LaserCount <= 0) return;
            Laser = new Laser(this, _configuration, () =>
            {
                Laser?.Dispose();
                Laser = null;
            });
            _objectsController.AddUpdatable(Laser);
            LaserCount--;
        }
    
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            //Movement
            if (_input.Ship.Move.IsPressed())
            {
                Acceleration = Rotation.RotationToVector() * _configuration.movingSpeed;
            }
            else
            {
                Acceleration = Vector2.zero;
            }

            Rotation -= _configuration.rotationSpeedSpeed * deltaTime * _input.Ship.Rotation.ReadValue<float>();

            //Teleportation
            if (Position.x > _topRightScreenPoint.x) Position = new Vector2(_bottomLeftScreenPoint.x, Position.y);
            if (Position.x < _bottomLeftScreenPoint.x) Position = new Vector2(_topRightScreenPoint.x, Position.y);
            if (Position.y > _topRightScreenPoint.x) Position = new Vector2( Position.x, _bottomLeftScreenPoint.y);
            if (Position.y < _bottomLeftScreenPoint.x) Position =  new Vector2( Position.x, _topRightScreenPoint.y);
        }
    
        protected override void OnCollision(PhysicBody other)
        {
            if (other is Asteroid or SmallAsteroid or Ufo)
            {
                other.Dispose();
                Dispose();
                return;
            }
        }
    }
}