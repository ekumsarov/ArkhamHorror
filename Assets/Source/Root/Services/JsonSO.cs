using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Sirenix.OdinInspector;

namespace EVI
{
    public abstract class JsonSO : SerializedScriptableObject, IJsonProcessing<JsonSO>
    {
        public void DeclareByJson(JSONNode node)
        {
            Declare(node);
        }

        protected abstract void Declare(JSONNode node);

        public JSONNode GetJSON()
        {
            return GetJSONExternal();
        }

        protected abstract JSONNode GetJSONExternal();

        public string GetJSONString() => GetJSON().ToString();
    }
}

