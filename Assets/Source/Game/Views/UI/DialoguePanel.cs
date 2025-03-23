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

        private EncounterNode _encounterNode;

        public void OpenDialogue(DialogueNode data)
        {
            
        }

        
        public void OpenDialogue(EncounterNode data)
        {
            gameObject.SetActive(true); // Показать панель
            _encounterNode = data;

            // 1) Устанавливаем текст
            _mainText.text = data.Text;

            // 2) Отображаем варианты (если их меньше, скрываем лишние)
            var choiceList = data.Choices;
            for (int i = 0; i < _choices.Count; i++)
            {
                if (i < choiceList.Count)
                {
                    _choices[i].gameObject.SetActive(true);
                    _choices[i].Setup(choiceList[i], this);
                }
                else
                {
                    _choices[i].gameObject.SetActive(false);
                }
            }
        }

        public void OnChoiceClicked(DialogueChoice choice)
        {
            if (_encounterNode != null)
            {
                
            }
            else
            {
                // На всякий случай, если нет currentNode
                CloseDialogue();
            }
        }

        /// <summary>
        /// Закрываем диалог (если EncounterNode сам не вызвал).
        /// </summary>
        public void CloseDialogue()
        {
            gameObject.SetActive(false);
        }
    }

}