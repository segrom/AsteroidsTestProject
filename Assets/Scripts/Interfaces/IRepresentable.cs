using Unity.VisualScripting;
using UnityEngine;

namespace Interfaces
{
    public interface IRepresentable
    {
        public Vector2 GetPosition();
        public float GetRotation();
        public Vector2 GetScale();
    }
}