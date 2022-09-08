using System;
using System.Collections.Generic;
using Interfaces;
using MonoBehaviours;
using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class ObjectsController: IDisposable
    {
        public Action<IUpdatable> OnDestroyObject { get; set; }
        
        private List<IUpdatable> _updatableObjects;
        private List<PhysicBody> _physicObjects;
        private RepresentationController _representationController;
        
        private double _lastUpdateTick = 0;
        private float _deltaTime = 0;

        public ObjectsController(RepresentationController representationController)
        {
            _representationController = representationController;
            _updatableObjects = new List<IUpdatable>();
            _physicObjects = new List<PhysicBody>();
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            if(updatable is IRepresentable representable) _representationController.CreateRepresentation(representable);

            _updatableObjects.Add(updatable);
        
            if(updatable is PhysicBody body) _physicObjects.Add(body);

            updatable.OnDestroy += DestroyObject;
        }

        private void DestroyObject(IUpdatable updatable)
        {
            OnDestroyObject?.Invoke(updatable);
            _updatableObjects.Remove(updatable);
            if(updatable is IRepresentable representable) _representationController.DeleteRepresentation(representable);
            if(updatable is PhysicBody body) _physicObjects.Remove(body);
        }

        public void UpdateAll()
        {
            _deltaTime = (float)(Time.realtimeSinceStartupAsDouble - _lastUpdateTick);
            for (int i = 0; i < _updatableObjects.Count; i++)
            {
                _updatableObjects[i].Update(_deltaTime);
            }

            _lastUpdateTick = Time.realtimeSinceStartupAsDouble;
        }

        public void DetectCollisions()
        {
            for (int i = 0; i < _physicObjects.Count; i++)
            {
                if(_physicObjects[i] is null) return;
                _physicObjects[i].DetectCollisions(_physicObjects);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _physicObjects.Count; i++)
            {
                if(_physicObjects[i] is null) return;
                _physicObjects[i].Dispose();
            }
        }
    }
}