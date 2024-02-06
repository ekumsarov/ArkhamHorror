using UnityEngine;
using SimpleJSON;
using System.IO;
using System;

namespace EVI
{
    public class SaveAndLoadHelper
    {
        private static string _basePath = "/Resources/Data/";
        public static void SaveData(string text, string fileName, string path = null, FileSystemEventHandler callback = null, bool persistent = true)
        {
            string finalPath = SaveAndLoadHelper.BuildPath(path, persistent);
            string finalpathAndFile = BuildPathAndName(path, fileName, persistent);
            CheckPath(finalPath);

            File.WriteAllText(finalpathAndFile, text);
        }

        public static JSONNode CheckAndLoadDataJSON(string fileName, string path = null, bool persistent = true)
        {
            string finalPath = BuildPathAndName(path, fileName, persistent);

            if (File.Exists(finalPath) == false)
                return null;

            string pathCrossString = File.ReadAllText(finalPath);
            JSONNode tempNode = JSON.Parse(pathCrossString);

            return tempNode;
        }

        public static bool CheckFileAtPath(string fileName, string path = null, bool persistent = true)
        {
            string finalPath = BuildPathAndName(path, fileName, persistent);
            return File.Exists(finalPath);
        }

        public static string BuildPath(string path, bool persistent)
        {
            string finalPath;

            if (persistent == true)
            {
                finalPath = Application.persistentDataPath + _basePath;
            }
            else
            {
                finalPath = Application.dataPath + _basePath;
            }

            if (string.IsNullOrEmpty(path) == false)
            {
                finalPath += path;
            }

            CheckPath(finalPath);

            return finalPath;
        }

        public static string BuildPathAndName(string path, string fileName, bool persistent)
        {
            string finalPathAndName;

            if (persistent == true)
            {
                finalPathAndName = Application.persistentDataPath + _basePath;
            }
            else
            {
                finalPathAndName = Application.dataPath + _basePath;
            }

            if (string.IsNullOrEmpty(path) == false)
            {
                finalPathAndName += path + fileName;
            }
            else
            {
                finalPathAndName += fileName;
            }

            return finalPathAndName;
        }

        public static void CheckPath(string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }

        public static bool CheckFilePath(string path)
        {
            return File.Exists(path);

        }
    }
}