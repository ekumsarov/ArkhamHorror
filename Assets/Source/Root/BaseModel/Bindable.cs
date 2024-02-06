using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Linq;
using SimpleJSON;

namespace EVI
{
    public class Bindable : JSONInitializer<Bindable>, IBindable
    {
        protected override Bindable Instance() => this;
        public IBindable BindableObject => this;

        private Dictionary<string, Callback> _fields;
        private bool _isBinded = false;
        public bool IsBinded => _isBinded;

        protected override void InitializeBase()
        {
            if (_isBinded == false)
            {
                PrepareBindings();
            }
        }
        
        protected override void InitializeParameter()
        {

        }

        protected override void SerializeJson(JSONNode node)
        {
            
        }

        protected override JSONNode GetSaveExternal()
        {
            return null;
        }

        private void PrepareBindings()
        {
            _fields = new Dictionary<string, Callback>();

            Type myType = this.GetType();

            foreach (var field in myType.GetFields(BindingFlags.NonPublic | BindingFlags.Public
            | BindingFlags.Instance | BindingFlags.Static))
            {
                if (field.GetCustomAttributes<BindablePropertyAttribute>().Count() == 0)
                    continue;

                if (_fields.ContainsKey(field.Name))
                    continue;

                _fields.Add(field.Name, new Callback(field.Name));
            }

            foreach (var field in myType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public
            | BindingFlags.Instance | BindingFlags.Static))
            {
                if (field.GetCustomAttributes<BindablePropertyAttribute>().Count() == 0)
                    continue;

                if (_fields.ContainsKey(field.Name))
                    continue;

                _fields.Add(field.Name, new Callback(field.Name));
            }

            foreach (var field in myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public
            | BindingFlags.Instance | BindingFlags.Static))
            {
                if (field.GetCustomAttributes<BindablePropertyAttribute>().Count() == 0)
                    continue;


                Debug.LogError(field.GetCustomAttributes<BindablePropertyAttribute>().First().TypeId);
                if (_fields.ContainsKey(field.Name))
                    continue;

                //_fields.Add(field.Name, new Callback(field.Name));
            }
        }

        public void InvokeChange<T>(string memberName, T obj)
        {
            if (_fields.ContainsKey(memberName) == false)
                return;

            _fields[memberName].TryInvoke(obj);
        }

        public void InvokeChange<T>(string memberName, T obj, Action callback)
        {
            if (_fields.ContainsKey(memberName) == false)
                return;

            _fields[memberName].TryInvoke(obj, callback);
        }

        public List<string> GetBinds => _fields.Keys.ToList();

        public void AddListiner(string name, Delegate del)
        {
            if (_fields.ContainsKey(name) == false || del == null)
                return;

            _fields[name].AddListener(del);
        }

        public void RemoveListener(string name, Delegate del)
        {
            if (_fields.ContainsKey(name) == false || del == null)
                return;

            _fields[name].RemoveListener(del);
        }
    }
}

