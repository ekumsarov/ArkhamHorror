using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace EVI
{
    public interface IJsonProcessing<T>
    {
        public string GetJSONString();

        public JSONNode GetJSON();

        /// <summary>
        /// Use this method may cause a problem for a reason of changing Object parameters 
        /// </summary>
        /// <param name="JSONNode"></param>
        public void DeclareByJson(JSONNode node);

        public static T DeclareByJSON(JSONNode node) => throw new System.Exception();
    }

}