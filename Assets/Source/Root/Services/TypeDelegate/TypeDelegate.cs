using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EVI
{
    public class TypeDelegate 
    {
        private Dictionary<TypeID, Delegate> _delegates;

        public void DeclareDelegate<T>(Action<T> callback)
        {
            if (_delegates == null)
                _delegates = new Dictionary<TypeID, Delegate>();

            DeclareDelegate(typeof(T), null, callback);
        }

        public void DeclareDelegate<T>(Action callback)
        {
            if (_delegates == null)
                _delegates = new Dictionary<TypeID, Delegate>();

            DeclareDelegate(typeof(T), null, callback);
        }

        public void Clear()
        {
            _delegates.Clear();
        }

        private void DeclareDelegate(Type detectType, object identifire, Delegate callback)
        {
            TypeID declareID = new TypeID(detectType, identifire);

            if (_delegates.ContainsKey(declareID))
                _delegates[declareID] = callback;
            else
                _delegates.Add(declareID, callback);
        }

        public void TryInvoke(object identifier)
        {
            if (_delegates == null)
                _delegates = new Dictionary<TypeID, Delegate>();

            foreach (TypeID id in _delegates.Keys)
            {
                if(id.Equals(identifier))
                {
                    if (_delegates.TryGetValue(id, out Delegate callback))
                    {
                        try
                        {
                            if (callback.Method.GetParameters().Length > 0)
                                callback.DynamicInvoke(identifier);
                            else
                                callback.DynamicInvoke();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error invoking callback for type {id}: {ex.Message}");
                        }
                    }
                }
                
            }
        }
    }
}