using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class TypedModel<T> : BaseModel where T : BaseView
    {
        private T _view;
        public T View => _view;

        public void SetupView(T view)
        {
            _view = view;
        }
    }
}