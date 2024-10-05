using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Sirenix.OdinInspector;

namespace EVI
{
    public abstract class SOInitializer<TClass> : SerializedScriptableObject, ISOInitializer<TClass>
    {
        #region Initialization
        private Dictionary<Type[], Delegate> _callbacksDictionary = new Dictionary<Type[], Delegate>(new TypeArrayComparer());

        protected abstract void InitializeBase();
        protected abstract TClass Instance();
        protected abstract JSONNode GetSaveExternal();
        protected abstract void SerializeJson(JSONNode node);
        protected abstract void SerializeSO(TClass so);

        public TClass Init(JSONNode node)
        {
            Init(this, node);
            return Instance();
        }

        public TClass Init(JSONNode node, params object[] args)
        {
            Init(this, node, args);
            return Instance();
        }

        public TClass Init(TClass node)
        {
            Init(this, node);
            return Instance();
        }

        public TClass Init(TClass node, params object[] args)
        {
            Init(this, node, args);
            return Instance();
        }

        public JSONNode GetSave()
        {
            return GetSaveExternal();
        }
        public string GetJSONString() => GetSave().ToString();

        protected void Init(in SOInitializer<TClass> value, JSONNode node)
        {
            value.SerializeJson(node);
            value.InitializeBase();
        }

        protected void Init(in SOInitializer<TClass> value, JSONNode node, params object[] args)
        {
            value.SerializeJson(node);
            value.InitializeParameter();
            value.InitializeBase();

            Type[] arguments = TypeArrayComparer.CreateFrom(args);

            if (_callbacksDictionary.ContainsKey(arguments) == false)
            {
                throw new ArgumentException("No callback with this types" + args.ToString());
            }

            try
            {
                var callback = _callbacksDictionary[arguments];
                if (callback == null)
                {
                    throw new ArgumentException("Not set method.");
                }

                callback.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Not set method. " + ex);
            }
        }
        
        protected void Init(in SOInitializer<TClass> value, TClass so)
        {
            value.SerializeSO(so);
            value.InitializeBase();
        }

        protected void Init(in SOInitializer<TClass> value, TClass so, params object[] args)
        {
            value.SerializeSO(so);
            value.InitializeParameter();
            value.InitializeBase();

            Type[] arguments = TypeArrayComparer.CreateFrom(args);

            if (_callbacksDictionary.ContainsKey(arguments) == false)
            {
                throw new ArgumentException("No callback with this types" + args.ToString());
            }

            try
            {
                var callback = _callbacksDictionary[arguments];
                if (callback == null)
                {
                    throw new ArgumentException("Not set method.");
                }

                callback.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Not set method. " + ex);
            }
        }
        
        protected abstract void InitializeParameter();

        protected void InitParameter<Tparam1>(Action<Tparam1> callback)
        {
            var paramTypes = new Type[] { typeof(Tparam1) };
            if (_callbacksDictionary.ContainsKey(paramTypes))
                return;

            _callbacksDictionary.Add(paramTypes, callback);
        }

        protected void InitParameter<Tparam1, Tparam2>(Action<Tparam1, Tparam2> callback)
        {
            var paramTypes = new Type[] { typeof(Tparam1), typeof(Tparam2) };
            if (_callbacksDictionary.ContainsKey(paramTypes))
                return;

            _callbacksDictionary.Add(paramTypes, callback);
        }

        protected void InitParameter<Tparam1, Tparam2, Tparam3>(Action<Tparam1, Tparam2, Tparam3> callback)
        {
            var paramTypes = new Type[] { typeof(Tparam1), typeof(Tparam2), typeof(Tparam3) };
            if (_callbacksDictionary.ContainsKey(paramTypes))
                return;

            _callbacksDictionary.Add(paramTypes, callback);
        }

        protected void InitParameter<Tparam1, Tparam2, Tparam3, Tparam4>(Action<Tparam1, Tparam2, Tparam3, Tparam4> callback)
        {
            var paramTypes = new Type[] { typeof(Tparam1), typeof(Tparam2), typeof(Tparam3), typeof(Tparam4) };
            if (_callbacksDictionary.ContainsKey(paramTypes))
                return;

            _callbacksDictionary.Add(paramTypes, callback);
        }
        #endregion
    }
}