using EVI.Game;
using SimpleJSON;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EVI
{
    [JSONSerializable]
    public class CampaignData : ScriptableObject
    {
        [SerializeField]
        private GameTime _gameTime = new GameTime();
        public GameTime Time => _gameTime;

        [SerializeField, JSONConvert, OnInspectorInit("UpdateCampaing")]
        private string _campaingName;

        [SerializeField, JSONConvert] private CardCell _mainLayout;
        public CardCell MainLayout => _mainLayout;

        [SerializeField, JSONConvert] private List<Location> _locations = new List<Location>();
        public List<Location> Locations => _locations;

        [SerializeField, JSONConvert] private List<GameCard> _gameCards = new List<GameCard>();
        public List<GameCard> GameCards => _gameCards;

        [SerializeField, JSONConvert] private List<Resource> _resources = new List<Resource>();
        public List<Resource> Resources => _resources;

        [Button]
        private void SaveCampaing([FolderPath] string toPath)
        {
            // Сериализация данных в JSON
            JSONNode node = SerializationModule.SerializeToJson(this);

            // Убедимся, что директория существует
            if (!Directory.Exists(toPath))
            {
                Directory.CreateDirectory(toPath);
            }

            // Определяем полный путь к файлу
            string filePath = Path.Combine(toPath, $"{_campaingName}.json");

            // Записываем JSON-данные в файл
            File.WriteAllText(filePath, node.ToString(2)); // '2' для красивого форматирования

            Debug.Log($"Кампания сохранена по пути: {filePath}");
        }

        private void UpdateCampaing()
        {
            // Получаем полный путь к директории, содержащей файл
            string fullPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            // Извлекаем только имя папки
            _campaingName = Path.GetFileName(fullPath);
        }
    }
}