using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVI.Game
{
    public class ResourceView : TypedView<Resource>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _label;

        protected override void InitializeBaseInternal()
        {
            base.InitializeBaseInternal();

            _label.text = Model.CurrentValue.ToString();
        }

        [BindTo]
        private void CurrentValueChanged()
        {
            _label.text = Model.CurrentValue.ToString();
        }
    }
}

