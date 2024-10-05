using System;
using System.Collections.Generic;
using EnumExtension;

namespace EVI
{
    public class Status<T> where T : Enum
    {
        private T _value;

        public T GetStatus => _value;

        public List<T> Values
        {
            get 
            {
                List<T> values = new List<T>();

                foreach(T item in Enum.GetValues(typeof(T)))
                {
                    if(_value.HasFlag(item))
                    {
                        values.Add(item);
                    }
                }

                return values;
            }
        }


        private Dictionary<T, List<T>> _exceptions;
        public void DeclareStatusException(T forType, T valutType)
        {
            if(_exceptions == null)
                _exceptions = new Dictionary<T, List<T>>();

            if(_exceptions.ContainsKey(forType))
            {
                if(_exceptions[forType].Contains(valutType))
                    return;
                
                _exceptions[forType].Add(valutType);
            }
            else
            {
                _exceptions.Add(forType, new List<T>());
                _exceptions[forType].Add(valutType);
            }
        }

        private Dictionary<T, List<T>> _otherwiseExceptions;
        public void DeclareOtherwiseStatusException(T forType, T valutType)
        {
            if(_otherwiseExceptions == null)
                _otherwiseExceptions = new Dictionary<T, List<T>>();

            if(_otherwiseExceptions.ContainsKey(forType))
            {
                if(_otherwiseExceptions[forType].Contains(valutType))
                    return;
                
                _otherwiseExceptions[forType].Add(valutType);
            }
            else
            {
                _otherwiseExceptions.Add(forType, new List<T>());
                _otherwiseExceptions[forType].Add(valutType);
            }
        }

        public virtual void AddStatus(T valutType)
        {
            if(_value.HasFlag(valutType))
                return;

            if(_exceptions == null)
                _exceptions = new Dictionary<T, List<T>>();

            if(_exceptions.ContainsKey(valutType))
            {
                foreach(T item in _exceptions[valutType])
                {
                    if(_value.HasFlag(item))
                        _value = _value.Remove(item);
                }
            }

            _value = _value.Include(valutType);
        }

        public virtual void RemoveStatus(T valutType)
        {
            if(_value.HasFlag(valutType) == false)
                return;

            if(_otherwiseExceptions == null)
                _otherwiseExceptions = new Dictionary<T, List<T>>();

            if(_otherwiseExceptions.ContainsKey(valutType))
            {
                foreach(T item in _otherwiseExceptions[valutType])
                {
                    if(_value.HasFlag(item) == false)
                        _value = _value.Include(item);
                }
            }

            _value = _value.Remove(valutType);
        }

        public void InstallStatus(T valutType)
        {
            _value = valutType;
        }

        public bool HasStatus(T valutType)
        {
            return _value.HasFlag(valutType);
        }
    }
}