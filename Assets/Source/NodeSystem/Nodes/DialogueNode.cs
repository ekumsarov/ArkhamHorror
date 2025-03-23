using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace EVI
{
    public class DialogueNode : LogicNode
    {
        [SerializeField]
        private LocalizedString _text;

        public string Text => _text.GetLocalizedString();

        [SerializeField]
        public List<DialogueChoice> Choices;


        public static DialogueNode Create(LocalizedString text, List<DialogueChoice> choices)
        {
            DialogueNode temp = ScriptableObject.CreateInstance<DialogueNode>();

            temp._text = text;
            temp.Choices = choices;
            foreach(var choice in temp.Choices)
            {
            }

            return temp;
        }
    }
}

