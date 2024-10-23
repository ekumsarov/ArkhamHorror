using UnityEngine;
using Sirenix.OdinInspector;

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
            if(string.IsNullOrEmpty(_id))
            {
                _id = this.name;
            }
        }
    }
}
