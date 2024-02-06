using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Zenject;

namespace EVI
{
    public class LevelDataHandler : MonoBehaviour, IInitializable
    {
        private LevelData _levelData;
        public LevelData Data => _levelData;

        private Action _callback;

        public void Initialize()
        {
            _levelData = LevelData.InstatinateByJSON(SaveAndLoadHelper.CheckAndLoadDataJSON("LevelData.json", "Save/Relic/"));
            _callback?.Invoke();
        }


        public void InitializeData(Action callback)
        {
            if (_levelData == null)
            {
                _callback = callback;
                return;
            }

            _callback = callback;
            _callback?.Invoke();
        }

        public void SaveData()
        {
            string pathAndName = SaveAndLoadHelper.BuildPathAndName("Save/Relic/", "LevelData.json", true);
            string path = SaveAndLoadHelper.BuildPath("Save/Relic/", true);
            SaveAndLoadHelper.CheckPath(path);

            JSONNode saveNode = _levelData.GetJSON();
            File.WriteAllText(pathAndName, saveNode.ToString());
        }
    }
}

