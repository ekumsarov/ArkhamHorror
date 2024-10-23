using EVI.Game;
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

        [SerializeField] private CardCell _mainLayout;
        public CardCell MainLayout => _mainLayout;

        [SerializeField] private List<Location> _locations = new List<Location>();
        public List<Location> Locations => _locations;

        [SerializeField] private List<GameCard> _gameCards = new List<GameCard>();
        public List<GameCard> GameCards => _gameCards;
    }
}