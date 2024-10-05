using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    public class RootTick : IInitializable, ITickable, IRegystryRoot, IFixedTickable
    {
        private List<IUpdatable> _updateList;
        private List<IUpdatable> _safeUpdate;
        private List<IUpdatable> _safeDelete;

        private List<IFixedUpdatable> _updateFixedList;
        private List<IFixedUpdatable> _safeFixedUpdate;
        private List<IFixedUpdatable> _safeFixedDelete;

        private float _deltaTime;

        public void Initialize()
        {
            _deltaTime = 0f;

            if (_updateList == null)
                _updateList = new List<IUpdatable>();

            if (_safeUpdate == null)
                _safeUpdate = new List<IUpdatable>();

            if (_safeDelete == null)
                _safeDelete = new List<IUpdatable>();

            if (_updateFixedList == null)
                _updateFixedList = new List<IFixedUpdatable>();

            if (_safeFixedDelete == null)
                _safeFixedDelete = new List<IFixedUpdatable>();

            if (_safeFixedUpdate == null)
                _safeFixedUpdate = new List<IFixedUpdatable>();
        }

        public void Tick()
        {
            _deltaTime = Time.deltaTime;

            foreach (var temp in _safeUpdate)
            {
                _updateList.Add(temp);
            }
            _safeUpdate.Clear();

            foreach (var temp in _safeDelete)
            {
                _updateList.Remove(temp);
            }
            _safeDelete.Clear();

            foreach (var temp in _updateList)
                temp.Tick(_deltaTime);
        }

        public void Registry(IUpdatable updatable)
        {
            if (_updateList == null)
                _updateList = new List<IUpdatable>();

            if (_updateList.Contains(updatable))
                return;

            if (_safeUpdate == null)
                _safeUpdate = new List<IUpdatable>();

            if (_safeUpdate.Contains(updatable))
                return;

            _safeUpdate.Add(updatable);
        }

        public void Unregisrty(IUpdatable updatable)
        {
            if (_safeDelete == null)
                _safeDelete = new List<IUpdatable>();

            if (_safeDelete.Contains(updatable) || _updateList.Contains(updatable) == false)
                return;

            _safeDelete.Add(updatable);
        }

        public void RegistryFixed(IFixedUpdatable updatable)
        {
            if (_updateFixedList == null)
                _updateFixedList = new List<IFixedUpdatable>();

            if (_updateFixedList.Contains(updatable))
                return;

            if (_safeFixedUpdate == null)
                _safeFixedUpdate = new List<IFixedUpdatable>();

            if (_safeFixedUpdate.Contains(updatable))
                return;

            _safeFixedUpdate.Add(updatable);
        }

        public void UnregisrtyFixed(IFixedUpdatable updatable)
        {
            if (_safeFixedDelete == null)
                _safeFixedDelete = new List<IFixedUpdatable>();

            if (_safeFixedDelete.Contains(updatable) || _updateFixedList.Contains(updatable) == false)
                return;

            _safeFixedDelete.Add(updatable);
        }

        public void FixedTick()
        {
            foreach (var temp in _safeFixedUpdate)
            {
                _updateFixedList.Add(temp);
            }
            _safeFixedUpdate.Clear();

            foreach (var temp in _safeFixedDelete)
            {
                _updateFixedList.Remove(temp);
            }
            _safeFixedDelete.Clear();

            foreach (var temp in _updateFixedList)
                temp.FixedTick();
        }
    }
}