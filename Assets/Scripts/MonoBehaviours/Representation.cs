using System;
using Entities;
using Interfaces;
using UnityEngine;

namespace MonoBehaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Representation: MonoBehaviour
    {
        public IRepresentable Representable { get; private set; }
        
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Setup(Sprite sprite, IRepresentable representable)
        {
            _spriteRenderer.sprite = sprite;
            Representable = representable;
        }

        public void Refresh(float deltaTime)
        {
            transform.position = Representable.GetPosition();
            transform.rotation = Quaternion.Euler(0,0, Representable.GetRotation());
            transform.localScale = Representable.GetScale();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (Representable is PhysicBody body)
            {
                Gizmos.DrawWireSphere(body.Position, body.Radius);
            }
        }
    }
}