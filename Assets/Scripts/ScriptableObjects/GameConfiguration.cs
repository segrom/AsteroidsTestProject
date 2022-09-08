using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Asteroids/Game Configuration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public int updateDelayMilliseconds;
        public ShipConfiguration shipConfiguration;
        [Space]
        public float asteroidSpeed;
        public float asteroidSpawnDelay;
        public float smallAsteroidSpeed;
        [Space]
        public float ufoSpeed;
        public float ufoSpawnDelay;
    }
}