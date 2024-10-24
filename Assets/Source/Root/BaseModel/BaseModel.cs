using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace EVI
{
    [JSONSerializable]
    public class BaseModel : SOBindable, IIdentifiable
    {
        [SerializeField, ReadOnly, JSONConvert, OnInspectorInit("BaseModelComponents")]
        private string _id;

        public string ID => _id;

        private void BaseModelComponents()
        {
            _id = this.name;
        }

        public Action<BaseModel> OnDestroyed;
        public void DestroyModel()
        {
            OnDestroyed?.Invoke(this);
            CleanUp();
            OnDestroyed = null;
        }

        protected virtual void CleanUp()
        {

        }
    }
}
