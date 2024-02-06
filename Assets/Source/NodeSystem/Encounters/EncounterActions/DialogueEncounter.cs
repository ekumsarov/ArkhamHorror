using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace EVI
{
    public class DialogueEncounter : EncounterAction
    {
        [SerializeField]
        private LocalizedString _text;

        public string Text => _text.GetLocalizedString();

        [SerializeField]
        public List<DialogueChoice> Choices;


        public static DialogueEncounter Create(LocalizedString text, List<DialogueChoice> choices)
        {
            DialogueEncounter temp = ScriptableObject.CreateInstance<DialogueEncounter>();

            temp._text = text;
            temp.Choices = choices;
            foreach(var choice in temp.Choices)
            {
                choice.BindEncounter(temp._parentEncounter);
            }

            return temp;
        }
    }
}

