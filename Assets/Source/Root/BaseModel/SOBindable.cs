using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Linq;
using SimpleJSON;

namespace EVI
{
    public class SOBindable : SOInitializer<SOBindable>, IBindable
    {
        protected override SOBindable Instance() => this;
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

            InitializeBaseExternal();
        }

        protected virtual void InitializeBaseExternal()
        {

        }

        protected override void InitializeParameter()
        {
        }

        protected override void SerializeSO(SOBindable so)
        {
            var fieldsAndProperties = GetFieldsAndPropertiesWithAttribute<JSONConvertAttribute>();
            foreach (var kvp in fieldsAndProperties)
            {
                string name = kvp.Key;
                var member = kvp.Value;
                object value = GetValue(member, so);
                SetValue(member, value);
            }
        }

        protected override void SerializeJson(JSONNode node)
        {
            var fieldsAndProperties = GetFieldsAndPropertiesWithAttribute<JSONConvertAttribute>();
            foreach (var kvp in fieldsAndProperties)
            {
                string name = kvp.Key;
                var member = kvp.Value;
                if (node[name] != null)
                {
                    Type memberType = GetMemberType(member);
                    object value = node[name].JsonToObject(memberType);
                    SetValue(member, value);
                }
            }
        }

        protected override JSONNode GetSaveExternal()
        {
            PrepareForSave();
            var node = new JSONObject();
            var fieldsAndProperties = GetFieldsAndPropertiesWithAttribute<JSONConvertAttribute>();
            foreach (var kvp in fieldsAndProperties)
            {
                string name = kvp.Key;
                var member = kvp.Value;
                object value = GetValue(member);
                node[name] = value.ObjectToJson();
            }
            return node;
        }

        protected virtual void PrepareForSave()
        {

        }

        private void PrepareBindings()
        {
            _fields = new Dictionary<string, Callback>();

            Type myType = this.GetType();

            foreach (var field in myType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (field.GetCustomAttributes<BindablePropertyAttribute>().Count() == 0)
                    continue;

                string fieldName = NormalizeName(field.Name);
                if (_fields.ContainsKey(fieldName))
                    continue;

                _fields.Add(fieldName, new Callback(field.Name));
            }

            foreach (var property in myType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (property.GetCustomAttributes<BindablePropertyAttribute>().Count() == 0)
                    continue;

                string propertyName = NormalizeName(property.Name);
                if (_fields.ContainsKey(propertyName))
                    continue;

                _fields.Add(propertyName, new Callback(property.Name));
            }

            foreach (var method in myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (method.GetCustomAttributes<BindableEventAttribute>().Count() == 0)
                    continue;

                string methodName = NormalizeName(method.Name);
                if (_fields.ContainsKey(methodName))
                    continue;

                _fields.Add(methodName, new Callback(method.Name));
            }
        }
        public void InvokeChange<T>(string memberName, T obj)
        {
            string normalizedMemberName = NormalizeName(memberName);

            if (!_fields.ContainsKey(normalizedMemberName))
                return;

            _fields[normalizedMemberName].TryInvoke(obj);
        }

        public void InvokeChange<T>(string memberName, T obj, Action callback)
        {
            string normalizedMemberName = NormalizeName(memberName);

            if (!_fields.ContainsKey(normalizedMemberName))
                return;

            _fields[normalizedMemberName].TryInvoke(obj, callback);
        }

        public void InvokeChange(string memberName, Action callback)
        {
            string normalizedMemberName = NormalizeName(memberName);

            if (!_fields.ContainsKey(normalizedMemberName))
                return;

            _fields[normalizedMemberName].TryInvoke(callback);
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

        private Dictionary<string, MemberInfo> GetFieldsAndPropertiesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            var result = new Dictionary<string, MemberInfo>();
            Type myType = this.GetType();

            while (myType != null && myType != typeof(object))
            {
                var fields = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                   .Where(field => field.GetCustomAttributes<TAttribute>().Any());

                var properties = myType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                                       .Where(prop => prop.GetCustomAttributes<TAttribute>().Any());

                foreach (var field in fields)
                {
                    result[field.Name] = field;
                }

                foreach (var prop in properties)
                {
                    result[prop.Name] = prop;
                }

                myType = myType.BaseType;
            }

            return result;
        }

        private object GetValue(MemberInfo member)
        {
            if (member is FieldInfo field)
            {
                return field.GetValue(this);
            }
            else if (member is PropertyInfo prop)
            {
                return prop.GetValue(this);
            }
            return null;
        }

        private void SetValue(MemberInfo member, object value)
        {
            if (member is FieldInfo field)
            {
                field.SetValue(this, value);
            }
            else if (member is PropertyInfo prop)
            {
                prop.SetValue(this, value);
            }
        }

        private Type GetMemberType(MemberInfo member)
        {
            if (member is FieldInfo field)
            {
                return field.FieldType;
            }
            else if (member is PropertyInfo prop)
            {
                return prop.PropertyType;
            }
            return null;
        }

        private object GetValue(MemberInfo member, object source)
        {
            if (member is FieldInfo field)
            {
                return field.GetValue(source);
            }
            else if (member is PropertyInfo prop)
            {
                return prop.GetValue(source);
            }
            return null;
        }

        private string NormalizeName(string name)
        {
            // Убираем все подчеркивания из имени
            return name.Replace("_", "").ToLower();
        }

    }
}
