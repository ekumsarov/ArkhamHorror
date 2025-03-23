using EVI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace EVI.Game
{
    public enum ResourceType
    {
        SimpleValue, // Just an integer
        MinMaxValue, // CurrentValue between min and max
        Percentage   // Treated as a percentage of min to max
    }

    [System.Serializable, JSONSerializable, CreateAssetMenu(menuName = "Models/Resource")]
    public partial class Resource : TypedModel<ResourceView>
    {
        [SerializeField, JSONConvert] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField, LabelText("Иконка"), JSONConvert]
        private Image sprite;

        [SerializeField, LabelText("Тип"), EnumToggleButtons, JSONConvert]
        private ResourceType resourceType;

        [SerializeField, LabelText("Текущее значение"), JSONConvert]
        private int currentValue;

        [SerializeField, ShowIf("@this.resourceType == ResourceType.MinMaxValue || this.resourceType == ResourceType.Percentage"), LabelText("Min"), JSONConvert]
        private int minValue = 0;

        [SerializeField, ShowIf("@this.resourceType == ResourceType.MinMaxValue || this.resourceType == ResourceType.Percentage"), LabelText("Max"), JSONConvert]
        private int maxValue = int.MaxValue;

        public ResourceType Type => resourceType;

        [BindableProperty]
        public int CurrentValue 
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, minValue, maxValue);

                if (resourceType == ResourceType.Percentage)
                    InvokeChange<int>(nameof(currentValue), (int)GetPercentage() * 100);
                else
                    InvokeChange<int>(nameof(currentValue), currentValue);
            }
        }

        public int MinValue
        {
            get => minValue;
            set => minValue = value;
        }

        public int MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        // Example method for percentage
        public float GetPercentage()
        {
            if (resourceType != ResourceType.Percentage) return 0f;
            if (maxValue == minValue) return 0f;
            return (float)(currentValue - minValue) / (maxValue - minValue);
        }

        protected override void InitializeBaseExternal()
        {
            base.InitializeBaseExternal();


        }
    }

}

