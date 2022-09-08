using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours
{
    public class RepresentationController: MonoBehaviour
    {
        private SpriteLibrary _library;
        private List<Representation> _representations;

        public void Setup(SpriteLibrary spriteLibrary)
        {
            _representations = new List<Representation>();
            _library = spriteLibrary;
        }

        private void Update()
        {
            for (int i = 0; i < _representations.Count; i++)
            {
                _representations[i].Refresh(Time.deltaTime);
            }
        }

        public void CreateRepresentation(IRepresentable representable, Sprite sprite = null)
        {
            sprite ??= _library.GetSpriteByRepresentable(representable);

            var obj = new GameObject();
            obj.transform.parent = transform;
            obj.transform.position = Vector3.one * 120f;
            obj.name = representable.GetType().Name;
            obj.AddComponent<SpriteRenderer>();
            var rep = obj.AddComponent<Representation>();
            rep.Setup(sprite, representable);
            _representations.Add(rep);
        }

        public void DeleteRepresentation(IRepresentable representable)
        {
            var rep = _representations.FirstOrDefault(r => r.Representable == representable);
            if(rep is null) return;
            _representations.Remove(rep);
            DestroyImmediate(rep.gameObject);
        }

        public void Clear()
        {
            for (int i = 0; i < _representations.Count; i++)
            {
                DestroyImmediate(_representations[i].gameObject);
            }

            _representations = new List<Representation>();
        }
    }
}