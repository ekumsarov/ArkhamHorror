using UnityEngine;
using System.Collections.Generic;
using System;
using SimpleJSON;
using System.Collections;

namespace EVI
{
    public class LevelData : IJsonProcessing<LevelData>
    {
        private string _path;
        public string Path => _path;

        private SceneData _data;
        public SceneData Data => _data;

        public static LevelData InstatinateByJSON(JSONNode node)
        {
            if (node == null)
            {
                throw new System.ArgumentNullException("Not found data");
            }

            LevelData temp = new LevelData();

            temp._path = node["Path"].Value;
            temp._data = SceneData.ReadData(temp._path);

            return temp;
        }

        public class LevelDataRewrite
        {
            public static void SetPath(in LevelData data, string path)
            {
                data._path = path;
            }
        }

        #region JSON work
        public void DeclareByJson(JSONNode node)
        {
            throw new System.NotImplementedException();
        }

        public static LevelData InstatinateByJSON(JSONNode node, bool withTreasure = false)
        {
            if (node == null)
            {
                throw new System.ArgumentNullException("Not found data");
            }

            LevelData temp = new LevelData();

            temp._path = node["Path"].Value;

            return temp;
        }

        public JSONNode GetJSON()
        {
            JSONNode tempNode = new JSONObject();

            tempNode.Add("Path", _path);

            return tempNode;
        }

        public string GetJSONString() => GetJSON().ToString();
        #endregion

        public static LevelData CreateDefault()
        {
            LevelData temp = new LevelData();

            temp._path = string.Empty;

            return temp;
        }
    }

}