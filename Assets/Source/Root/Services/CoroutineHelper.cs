using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    public class CoroutineHelper : MonoBehaviour, IInitializable, ITickable
    {
        private List<IEnumerator> _coroutines;

        public void StartExternCoroutine(IEnumerator coroutine)
        {
            if(_coroutines == null)
            {
                _coroutines = new List<IEnumerator>();
            }

            if(_coroutines.Contains(coroutine))
                return;

            _coroutines.Add(coroutine);
            StartCoroutine(coroutine);
        }

        public void RemoveExternCoroutine(IEnumerator coroutine)
        {
            if(_coroutines == null)
            {
                _coroutines = new List<IEnumerator>();
            }

            Debug.LogError("Coroutine count: " + _coroutines.Count + "\n" + coroutine.ToString()); 

            if(_coroutines.Contains(coroutine) == false)
                return;

            StopCoroutine(coroutine);
            _coroutines.Remove(coroutine);
        }

        public void Initialize()
        {
            if(_invokes == null)
                _invokes = new List<MyTuple<Action, float>>();

            if(_safeAdd == null)
                _safeAdd = new List<MyTuple<Action, float>>();

            if(_safeDelete == null)
                _safeDelete = new List<MyTuple<Action, float>>();
        }

        public void InvokeCall(Action action, float timer)
        {
            _safeAdd.Add(new MyTuple<Action, float>(action, timer));
        }

        public void Tick()
        {
            foreach(var invoke in _safeAdd)
            {
                _invokes.Add(invoke);
            }
            _safeAdd.Clear();

            float delta = Time.deltaTime;
            foreach(var invoke in _invokes)
            {
                invoke.Item2 -= delta;
                if(invoke.Item2 <= 0)
                {
                    _safeDelete.Add(invoke);
                    invoke.Item1?.Invoke();
                }
            }

            foreach(var invoke in _safeDelete)
            {
                _invokes.Remove(invoke);
            }
            _safeDelete.Clear();
        }

        private List<MyTuple<Action, float>> _invokes;
        private List<MyTuple<Action, float>> _safeAdd;
        private List<MyTuple<Action, float>> _safeDelete;
    }
}
