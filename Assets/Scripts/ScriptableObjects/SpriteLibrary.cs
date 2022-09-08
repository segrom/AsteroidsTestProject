using System;
using Entities;
using Interfaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewSpriteLibrary", menuName = "Asteroids/Sprite Library", order = 0)]
    public class SpriteLibrary : ScriptableObject
    {
        public Sprite shipSprite;
        public Sprite asteroidSprite;
        public Sprite smallAsteroidSprite;
        public Sprite ufoSprite;
        public Sprite bulletSprite;
        public Sprite laserSprite;
        
        public Sprite GetSpriteByRepresentable(IRepresentable representable)
        {
            return representable switch
            {
                Ship => shipSprite,
                Bullet => bulletSprite,
                Asteroid => asteroidSprite,
                SmallAsteroid => smallAsteroidSprite,
                Ufo => ufoSprite,
                Laser => laserSprite,
                _ => throw new Exception("404 Sprite not found")
            };
        }
    }
}