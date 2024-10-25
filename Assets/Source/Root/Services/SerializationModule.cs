using SimpleJSON;
using System;
using System.IO;
using UnityEngine;

namespace EVI
{
    public static class SerializationModule
    {
        // Сериализация в JSON через расширение SimpleJSON
        public static JSONNode SerializeToJson(object obj)
        {
            try
            {
                return obj.ObjectToJson();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка сериализации: {ex.Message}");
                return JSONNull.CreateOrGet();
            }
        }

        // Десериализация из JSON
        public static T DeserializeFromJson<T>(JSONNode node)
        {
            try
            {
                return (T)node.JsonToObject(typeof(T));
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка десериализации: {ex.Message}");
                return default;
            }
        }

        public static T DeserializeFromJsonPath<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Debug.LogError($"Файл не найден: {path}");
                    return default;
                }

                string jsonContent = File.ReadAllText(path);

                // Парсим JSON.
                JSONNode node = JSON.Parse(jsonContent);

                if (node == null)
                {
                    Debug.LogError("Ошибка: Невозможно разобрать JSON.");
                    return default;
                }

                // Десериализуем объект из JSON.
                return (T)node.JsonToObject(typeof(T));
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка десериализации: {ex.Message}");
                return default;
            }
        }
    }

}
