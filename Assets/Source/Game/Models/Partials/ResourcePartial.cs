using EVI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace EVI.Game
{
    public partial class Resource 
    {
        public static Resource Create(string id, ResourceType type, int current, int min = 0, int max = int.MaxValue, Image sprite = null)
        {
            var resource = ScriptableObject.CreateInstance<Resource>();

            resource.name = id; // это важно, т.к. BaseModelComponents() берет id из имени объекта
            resource.sprite = sprite;
            resource.resourceType = type;
            resource.minValue = min;
            resource.maxValue = max;
            resource.CurrentValue = current; // уже с ограничением через setter

            return resource;
        }
    }

}

