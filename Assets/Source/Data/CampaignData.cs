using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class CampaignData : ScriptableObject
    {
        [SerializeField]
        private GameTime _gameTime = new GameTime();
        public GameTime Time => _gameTime;

        [SerializeField]
        private string _campaingName;
    }
}