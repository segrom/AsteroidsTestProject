using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShipConfiguration", menuName = "Asteroids/Ship Configuration", order = 0)]
    public class ShipConfiguration : ScriptableObject
    {
        public float movingSpeed;
        public float rotationSpeedSpeed;
        public float bulletsSpeed;
        [Space] 
        public float laserRadius;
        public int laserCount;
        [Tooltip("Delay between laser shot in seconds")]
        public int laserDelay;
        [Tooltip("Laser shot duration in seconds")]
        public int laserDuration;
    }
}