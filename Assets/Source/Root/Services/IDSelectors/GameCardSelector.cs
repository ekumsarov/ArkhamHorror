using System.Collections.Generic;
using EVI.Game;
using UnityEngine;
using System.Linq;

namespace EVI
{
    public static class GameCardSelector
    {
        public static IEnumerable<string> GetAllGameCardIDs()
        {
            // Загружаем все объекты GameCard из папки Resources "GameCards"
            var gameCards = Resources.LoadAll<GameCard>("Data");

            // Возвращаем список их ID
            return gameCards.Select(card => card.ID).ToList();
        }

        public static IEnumerable<string> GetMonsterCardIDs()
        {
            // Загружаем все объекты GameCard из папки Resources "GameCards"
            var gameCards = Resources.LoadAll<MonsterCard>("Data");

            // Возвращаем список их ID
            return gameCards.Select(card => card.ID).ToList();
        }

        public static IEnumerable<string> GetPlayerCardIDs()
        {
            // Загружаем все объекты GameCard из папки Resources "GameCards"
            var gameCards = Resources.LoadAll<PlayerCard>("Data");

            // Возвращаем список их ID
            return gameCards.Select(card => card.ID).ToList();
        }
    }
}