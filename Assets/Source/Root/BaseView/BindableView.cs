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

        private IBindable _bindableModel;
        protected IBindable BindableModel => _bindableModel;

        private Dictionary<string, Callback> _fields;

        protected override void InitializeBase(IBindable bindableModel)
        {
            _bindableModel = bindableModel;
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
            List<string> keys = _bindableModel.GetBinds;

            foreach (var method in myType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindToAttribute>().Count() == 0)
                    continue;

                foreach (var key in keys)
                {
                    if (NamesMatch(method.Name, key))
                    {
                        CreateAndAddListener(method, key);
                    }
                }
            }

            foreach (var method in myType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindEventAttribute>().Count() == 0)
                    continue;

                string normalizedMethodName = NormalizeName(method.Name.Replace("On", ""));

                foreach (var key in keys)
                {
                    if (NamesMatch(normalizedMethodName, key))
                    {
                        CreateAndAddListener(method, key);
                    }
                }
            }
        }

        public void Unbind()
        {
            Type myType = this.GetType();
            List<string> keys = _bindableModel.GetBinds;

            foreach (var method in myType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindToAttribute>().Count() == 0)
                    continue;

                foreach (var key in keys)
                {
                    if (NamesMatch(method.Name, key))
                    {
                        RemoveListener(method, key);
                    }
                }
            }
        }

        public void BindProcess()
        {
            _fields = new Dictionary<string, Callback>();
            Type myType = this.GetType();

            foreach (var method in myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public |
                                                     BindingFlags.Instance | BindingFlags.Static))
            {
                if (method.GetCustomAttributes<BindableProcessAttribute>().Count() == 0)
                    continue;

                string methodName = NormalizeName(method.Name);
                if (_fields.ContainsKey(methodName))
                    continue;

                _fields.Add(methodName, new Callback(method.Name));
            }

            Type modelType = _bindableModel.GetType();
            foreach (var method in modelType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic |
                                                        BindingFlags.Public | BindingFlags.Default))
            {
                if (method.GetCustomAttributes<BindProcessAttribute>().Count() == 0)
                    continue;

                foreach (var key in _fields.Keys)
                {
                    if (NamesMatch(method.Name, key))
                    {
                        CreateAndAddListener(method, key, _bindableModel);
                    }
                }
            }
        }

        private bool NamesMatch(string name1, string name2)
        {
            return NormalizeName(name1).Contains(NormalizeName(name2));
        }

        private string NormalizeName(string name)
        {
            // Убираем все подчеркивания и приводим к нижнему регистру
            return name.Replace("_", "").ToLower();
        }

        private void CreateAndAddListener(MethodInfo method, string key, object target = null)
        {
            var parameters = method.GetParameters();
            var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;

            if (parameterType == null)
            {
                var genericDelegate = Delegate.CreateDelegate(typeof(Action), target ?? this, method);
                _bindableModel.AddListiner(key, genericDelegate);
            }
            else
            {
                var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                var genericDelegate = Delegate.CreateDelegate(delegateType, target ?? this, method);
                _bindableModel.AddListiner(key, genericDelegate);
            }
        }

        private void RemoveListener(MethodInfo method, string key)
        {
            var parameters = method.GetParameters();
            var parameterType = parameters.Length == 1 ? parameters[0].ParameterType : null;

            if (parameterType == null)
            {
                var genericDelegate = Delegate.CreateDelegate(typeof(Action), this, method);
                _bindableModel.RemoveListener(key, genericDelegate);
            }
            else
            {
                var delegateType = typeof(Action<>).MakeGenericType(parameterType);
                var genericDelegate = Delegate.CreateDelegate(delegateType, this, method);
                _bindableModel.RemoveListener(key, genericDelegate);
            }
        }

        protected void InvokeProcess<T>(string key, T obj)
        {
            if (_fields.ContainsKey(NormalizeName(key)) == false)
                return;

            _fields[NormalizeName(key)].TryInvoke(obj);
        }
    }
}
