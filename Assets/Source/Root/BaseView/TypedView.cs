using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVI
{
    public class TypedView<T> : BaseView where T : BaseModel
    {
        private T _model;
        public T Model
        {
            get
            {
                if(_model == null)
                {
                    _model = BindableModel as T;
                    if(_model == null)
                        throw new System.Exception("No type for model " + typeof(T).ToString());
                }

                return _model;
            }
        }

        protected override void DestroyView(BaseModel model)
        {
            base.DestroyView(model);

            _model = null;
        }
    }
}