using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;

namespace EVI
{
    [CreateAssetMenu(menuName = "EOT/EnumGenerator")]
    public class EnumGenerator : ScriptableObject
    {
        private const string EnumFilePath = "Assets/Source/Generated/StatTypeEnum.cs";
        private const string EnumPath = "Assets/Source/Generated/";

        [SerializeField] private List<string> statNames = new List<string>();

        [Button("Actualize Enum")]
        public void ActualizeEnum()
        {
            GenerateEnum();
        }

        // Генерация enum файла
        private void GenerateEnum()
        {
            if(Directory.Exists(EnumPath) == false)
            {
                Directory.CreateDirectory(EnumPath);
            }

            using (StreamWriter writer = new StreamWriter(EnumFilePath))
            {
                writer.WriteLine("// Этот файл был сгенерирован автоматически.");
                writer.WriteLine("public enum StatType");
                writer.WriteLine("{");
                foreach (string statName in statNames)
                {
                    string enumEntry = MakeEnumFriendly(statName);
                    writer.WriteLine($"    {enumEntry},");
                }
                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
            Debug.Log("Enum был сгенерирован по пути: " + EnumFilePath);
        }

        // Приведение строк к безопасному для enum виду
        private static string MakeEnumFriendly(string name)
        {
            return name.Replace(" ", "_").Replace("-", "_").Replace(".", "_");
        }
    }
}

