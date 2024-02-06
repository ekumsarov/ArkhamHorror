using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    public class RootTick : IInitializable, ITickable, IRegystryRoot
    {
        private List<IUpdatable> _updateList;
        private List<IUpdatable> _safeUpdate;
        private List<IUpdatable> _safeDelete;

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
    }
}