using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EVI
{
    public class BindableView : ViewSerializer<BindableView>, IBinder
    {
        protected override BindableView Instance() => this;

        private IBindable _model;
        protected IBindable Model => _model;

        protected override void InitializeBase(IBindable bindableModel)
        {
            _model = bindableModel;
            Bind();
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

                string name = method.Name.Replace("Changed", "");

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

                string name = method.Name.Replace("Changed", "");

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
    }
}

