using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EVI
{
    public class ColliderDetect : MonoBehaviour
    {
        private Dictionary<ColliderDetectId, Delegate> _declarations;

        public void DeclareDetection<T>(Action<T> callback)
        {
            if (_declarations == null)
                _declarations = new Dictionary<ColliderDetectId, Delegate>();

            DeclareDetection(typeof(T), null, callback);
        }

        public void DeclareDetection<T>(Action callback)
        {
            if (_declarations == null)
                _declarations = new Dictionary<ColliderDetectId, Delegate>();

            DeclareDetection(typeof(T), null, callback);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            DetectCollider(collision.gameObject);
        }

        private void DeclareDetection(Type detectType, object identifire, Delegate callback)
        {
            ColliderDetectId declareID = new ColliderDetectId(detectType, identifire);

            if (_declarations.ContainsKey(declareID))
                _declarations[declareID] = callback;
            else
                _declarations.Add(declareID, callback);
        }

        private void DetectCollider(GameObject declaration)
        {
            if (_declarations == null)
                _declarations = new Dictionary<ColliderDetectId, Delegate>();

            foreach (ColliderDetectId id in _declarations.Keys)
            {
                Type detectType = id.Type;
                if (declaration.TryGetComponent(detectType, out Component component))
                {
                    ColliderDetectId declareId = new ColliderDetectId(detectType, null);

                    if (_declarations.TryGetValue(declareId, out Delegate callback))
                    {
                        try
                        {
                            if (callback.Method.GetParameters().Length > 0)
                                callback.DynamicInvoke(component);
                            else
                                callback.DynamicInvoke();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error invoking callback for type {detectType}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}