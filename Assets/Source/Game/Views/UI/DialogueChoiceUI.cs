using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVI
{
    public class DialogueChoiceUI : MonoBehaviour
    {
        [SerializeField, OnInspectorInit("UpdateComponents")] private Button _button;

        [SerializeField]
        private TextMeshProUGUI _text;

        private DialogueChoice _choice;
        private DialoguePanel _panel;

        public void Setup(DialogueChoice choice, DialoguePanel panel)
        {
            _choice = choice;
            _panel = panel;

            _text.text = _choice.DisplayedText;

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            // Уведомляем панель
            _panel.OnChoiceClicked(_choice);
        }

        private void UpdateComponents()
        {
            if(_button == null)
                _button = GetComponent<Button>();

            if(_text == null)
                _text = GetComponent<TextMeshProUGUI>();
        }
    }
}