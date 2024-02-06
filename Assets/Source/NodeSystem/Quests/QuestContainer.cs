using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EVI
{
    public class QuestContainer : SerializedScriptableObject
    {
        [SerializeField, ReadOnly]
        private List<QuestNode> _questParts;

        [SerializeField, HideInInspector]
        private string _path;

        public static QuestContainer Create(string path)
        {
            QuestContainer temp = ScriptableObject.CreateInstance<QuestContainer>();

            temp._path = path;

            return temp;
        }

        [Button]
        private void CreateQuestPart(string part)
        {
            string myPath = _path + "/" + part;
            if (Directory.Exists(myPath))
            {
                return;
            }

            Directory.CreateDirectory(myPath);

            if (_questParts == null)
                _questParts = new List<QuestNode>();

            var obj = QuestNode.Create(part, myPath);
            _questParts.Add(obj);
            AssetDatabase.CreateAsset(obj, myPath + "/" + part + ".asset");
            AssetDatabase.Refresh();
        }
    }
}

