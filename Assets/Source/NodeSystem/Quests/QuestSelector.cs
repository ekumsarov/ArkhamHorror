#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.Utilities;

namespace EVI
{
    public class QuestSelector : NodeEditorSelector
    {
        [SerializeField]
        private string _questName;

        public static new QuestSelector Create(string path, string missionName)
        {
            QuestSelector temp = ScriptableObject.CreateInstance<QuestSelector>();

            temp._parentFolder = path;
            temp.name = missionName;
            if (Directory.Exists(temp._parentFolder))
                return AssetDatabase.LoadAssetAtPath<QuestSelector>(temp._parentFolder + "/" + temp.name);
            else
                Directory.CreateDirectory(temp._parentFolder);

            AssetDatabase.CreateAsset(temp, temp._parentFolder + "/" + temp.name + ".asset");
            return temp;
        }

        [Button]
        private void CreateQuest()
        {
            if (_questName.Equals("Quest with this name already exist. Try another name"))
                return;

            if(Directory.Exists(_parentFolder + "/" + _questName))
            {
                _questName = "Quest with this name already exist. Try another name";
                return;
            }

            Directory.CreateDirectory(_parentFolder + "/" + _questName);

            var obj = QuestContainer.Create(_parentFolder + "/" + _questName);
            AssetDatabase.CreateAsset(obj, _parentFolder + "/" + _questName + "/" + "QuestContainer.asset");
            AssetDatabase.Refresh();
        }
    }
}
#endif