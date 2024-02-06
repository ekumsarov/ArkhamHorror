using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVI
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _mainText;

        [SerializeField]
        private List<DialogueChoiceUI> _choices;

        public void OpenDialogue(DialogueEncounter data)
        {
            
        }
    }
}