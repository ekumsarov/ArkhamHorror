using EVI.Game;
using SimpleJSON;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace EVI
{
    [CreateAssetMenu(menuName = "Data", fileName = "GameCard")]
    public class GameCardData : JsonSO
    {
        [SerializeField]
        private Sprite _icon;
        public Sprite Icon => _icon;
        [SerializeField]
        public int HealthPoints { get; private set; }
        [SerializeField]
        public int MindPoints { get; private set; }
        [SerializeField]
        public SkillGroup Skills { get; private set; }
        [SerializeField]
        public CardType CardType { get; private set; }

        protected override void Declare(JSONNode node)
        {
            
        }

        protected override JSONNode GetJSONExternal()
        {
            JSONObject node = new JSONObject();
            return node;
        }

        private void InspectorInit()
        {
            //if (_icon == null)
            //    _icon = Resources.LoadAll<Sprite>("Media/Sprites/Cards").FirstOrDefault(sp => sp.name.Equals("BaseCard"));
        }
    }
}

