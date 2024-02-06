using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EVI.Game
{
    public class Location : BaseModel
    {
        [SerializeField]
        private string _locationName;
        public string LocationName => _locationName;

        [SerializeField]
        List<CardPlace> _cardPlaces;

        protected override void SerializeJson(JSONNode node)
        {
            
        }

        protected override JSONNode GetSaveExternal()
        {
            JSONNode temp = new JSONObject();

            temp.Add(nameof(LocationName), _locationName);

            return temp;
        }
    }
}
