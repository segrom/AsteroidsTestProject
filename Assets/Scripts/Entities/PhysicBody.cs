using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Entities
{
    public class PhysicBody: IRepresentable, IUpdatable, IDisposable
    {
        public event IUpdatable.DestroyEvent OnDestroy;
        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }
    
        public float Rotation { get; set; }
    
        public virtual float Radius { get; set; }
        public void Simulate(float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
            Position += Velocity * deltaTime;
        }

        public void DetectCollisions(List<PhysicBody> physicBodies)
        {
            for (int i = 0; i < physicBodies.Count; i++)
            {
                var body = physicBodies[i];
                if(body == this || body is null) continue;
                if(Vector2.Distance(body.Position , Position) < body.Radius + Radius) OnCollision(body); 
            }
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public float GetRotation() => Rotation;
        
        public virtual Vector2 GetScale() => Vector2.one;
        
        public virtual void Update(float deltaTime)
        {
            Simulate(deltaTime);
        }

        public virtual void Dispose()
        {
            OnDestroy?.Invoke(this);
        }
    
        protected virtual void OnCollision(PhysicBody other){}
    }
}