using UnityEngine;
using Sirenix.OdinInspector;

namespace EVI
{
    [JSONSerializable]
    public class BaseModel : SOBindable, IIdentifiable
    {
        public string ID => name;
    }
}
