using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EVI
{
    [JSONSerializable]
    public class BindableView : ViewSerializer<BindableView>, IBinder
    {
        protected override BindableView Instance() => this;

        private IBindable _model;
        protected IBindable Model => _model;

        protected override void InitializeBase(IBindable bindableModel)
        {
            _model = bindableModel;
            Bind();
            BindProcess();
            InitializeBaseInternal();
        }

        protected virtual void InitializeBaseInternal()
        {

        }

        protected override void InitializeParameter()
        {
        }

        public void Bind()
        {
            Type myType = this.GetType();
            List<string> keys = _model.GetBinds;
            foreach(var method in myType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindToAttribute>().Count() == 0)
                    continue;

                foreach(var key in keys)
                {
                    if(method.Name.Contains(key))
                    {
                        var parameters = method.GetParameters();

                        var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;
                        if (parameterType == null)
                        {
                            var genericDelegate = Delegate.CreateDelegate(typeof(Action), this, method);

                            _model.AddListiner(key, genericDelegate);
                        }
                        else
                        {
                            var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                            var genericDelegate = Delegate.CreateDelegate(delegateType, this, method);

                            _model.AddListiner(key, genericDelegate);
                        }
                    }
                }
            }

            foreach(var method in myType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindEventAttribute>().Count() == 0)
                    continue;

                string name = method.Name.Replace("On", "");

                foreach(var key in keys)
                {
                    if(method.Name.Contains(key))
                    {
                        var parameters = method.GetParameters();

                        var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;
                        if (parameterType == null)
                        {
                            var genericDelegate = Delegate.CreateDelegate(typeof(Action), this, method);

                            _model.AddListiner(key, genericDelegate);
                        }
                        else
                        {
                            var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                            var genericDelegate = Delegate.CreateDelegate(delegateType, this, method);

                            _model.AddListiner(key, genericDelegate);
                        }
                    }
                }
            }
        }

        public void Unbind()
        {
            Type myType = this.GetType();
            List<string> keys = _model.GetBinds;
            foreach (var method in myType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindToAttribute>().Count() == 0)
                    continue;

                foreach (var key in keys)
                {
                    if (method.Name.Contains(key))
                    {
                        var parameters = method.GetParameters();

                        var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;
                        if (parameterType == null)
                        {
                            var genericDelegate = Delegate.CreateDelegate(typeof(Action), this, method);

                            _model.RemoveListener(key, genericDelegate);
                        }
                        else
                        {
                            var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                            var genericDelegate = Delegate.CreateDelegate(delegateType, this, method);

                            _model.RemoveListener(key, genericDelegate);
                        }
                    }
                }
            }

            foreach (var method in myType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindEventAttribute>().Count() == 0)
                    continue;

                foreach (var key in keys)
                {
                    if (method.Name.Contains(key))
                    {
                        var parameters = method.GetParameters();

                        var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;
                        if (parameterType == null)
                        {
                            var genericDelegate = Delegate.CreateDelegate(typeof(Action), this, method);

                            _model.RemoveListener(key, genericDelegate);
                        }
                        else
                        {
                            var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                            var genericDelegate = Delegate.CreateDelegate(delegateType, this, method);

                            _model.RemoveListener(key, genericDelegate);
                        }
                    }
                }
            }
        }

        private Dictionary<string, Callback> _fields;

        public void BindProcess()
        {
            _fields = new Dictionary<string, Callback>();

            Type myType = this.GetType();

            foreach (var field in myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public
            | BindingFlags.Instance | BindingFlags.Static))
            {
                if (field.GetCustomAttributes<BindableProcessAttribute>().Count() == 0)
                    continue;

                if (_fields.ContainsKey(field.Name))
                    continue;

                _fields.Add(field.Name, new Callback(field.Name));
            }

            Type modelType = _model.GetType();
            foreach(var method in modelType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindProcessAttribute>().Count() == 0)
                    continue;

                foreach(var key in _fields.Keys)
                {
                    if(method.Name.Contains(key))
                    {
                        var parameters = method.GetParameters();

                        var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;
                        if (parameterType == null)
                        {
                            var genericDelegate = Delegate.CreateDelegate(typeof(Action), _model, method);

                            AddListener(key, genericDelegate);
                        }
                        else
                        {
                            var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                            var genericDelegate = Delegate.CreateDelegate(delegateType, _model, method);

                            AddListener(key, genericDelegate);
                        }
                    }
                }
            }
        }
        
        private void AddListener(string key, Delegate del)
        {
            if (_fields.ContainsKey(key) == false || del == null)
                return;

            _fields[key].AddListener(del);
        }

        protected void InvokeProcess<T>(string key, T obj)
        {
            if (_fields.ContainsKey(key) == false)
                return;

            _fields[key].TryInvoke(obj);
        }
    }
}

