using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SimpleJSON;

namespace EVI
{
    public interface IInitializer<TClass>
    {
        public TClass Init();
        public TClass Init(params object[] args);
    }

    public interface IViewInitializer<TClass>
    {
        public TClass Init(IBindable bindable);
        public TClass Init(IBindable bindable, params object[] args);
    }

    public interface IJsonInitializer<TClass>
    {
        public TClass Init(JSONNode node);
        public TClass Init(JSONNode node, params object[] args);
        public JSONNode GetSave();
        public string GetJSONString();
    }
}

