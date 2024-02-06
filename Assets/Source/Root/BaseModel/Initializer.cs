using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public abstract class Initializer<TClass> : IInitializer<TClass>
    {
        #region Initialization
        private Dictionary<Type[], Delegate> _callbacksDictionary = new Dictionary<Type[], Delegate>(new TypeArrayComparer());

        protected abstract void InitializeBase();
        protected abstract TClass Instance();

        public TClass Init()
        {
            Init(this);
            return Instance();
        }

        public TClass Init(params object[] args)
        {
            Init(this, args);
            return Instance();
        }

        protected void Init(in Initializer<TClass> value)
        {
            value.InitializeBase();
        }

        protected void Init(in Initializer<TClass> value, params object[] args)
        {
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


