using UnityEngine;
using Zenject;
using System;
using SimpleJSON;

namespace EVI
{
    public class Instatinator : MonoBehaviour
    {
        [Inject] DiContainer _container;

        public T CreateInstance<T>()
        {
            return _container.Instantiate<T>();
        }


        public T GetModel<T>(JSONNode data)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1>(JSONNode data, Tparam1 tparam1)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data, tparam1);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1, Tparam2>(JSONNode data, Tparam1 tparam1, Tparam2 param2)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data, tparam1, param2);
            _container.Inject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1, Tparam2, Tparam3>(JSONNode data, Tparam1 tparam1, Tparam2 param2, Tparam3 param3)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data, tparam1, param2, param3);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T>(T data) where T : BaseModel
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init((data));
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1>(T data, Tparam1 tparam1) where T : BaseModel
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data, tparam1);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1, Tparam2>(T data, Tparam1 tparam1, Tparam2 param2) where T : BaseModel
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data, tparam1, param2);
            _container.Inject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1, Tparam2, Tparam3>(T data, Tparam1 tparam1, Tparam2 param2, Tparam3 param3) where T : BaseModel
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(data, tparam1, param2, param3);
            _container.QueueForInject(temp);

            return temp;
        }

        public void InstatinatePresenter(GameObject prefab, BaseModel model)
        {
            BaseView presenter = _container.InstantiatePrefab(prefab).GetComponent<BaseView>();
            presenter.Init(model);
        }

        public BaseView InstatinateAndGetPresenter(GameObject prefab, BaseModel model)
        {
            BaseView presenter = _container.InstantiatePrefab(prefab).GetComponent<BaseView>();
            presenter.Init(model);
            return presenter;
        }

        public T InstatinateAndGetPresenter<T>(GameObject prefab, BaseModel model) where T : BaseView
        {
            BaseView presenter = _container.InstantiatePrefab(prefab).GetComponent<BaseView>();
            presenter.Init(model);
            return presenter as T;
        }

        public void InstatinatePresenter(GameObject prefab, BaseModel model, params object[] args)
        {
            BaseView presenter = _container.InstantiatePrefab(prefab).GetComponent<BaseView>();
            presenter.Init(model, args);
        }

        public T GetCompleteCharacterModel<T>(GameObject prefab, BaseModel model, params object[] args) where T : BaseView
        {
            BaseView presenter = _container.InstantiatePrefab(prefab).GetComponent<BaseView>();
            if (args != null && args.Length > 0)
                presenter.Init(model, args);
            else
                presenter.Init(model);

            return presenter as T;
        }


        public T InstatinatePrefab<T>(GameObject prefab)
        {
            return _container.InstantiatePrefab(prefab).GetComponent<T>();
        }

        public void QueueForInject(object forInject)
        {
            _container.QueueForInject(forInject);
        }

        public void Inject(object forInject)
        {
            _container.Inject(forInject);
        }

        public T InstatinateSO<T>(T scriptableObject) where T : ScriptableObject
        {
            T temp = ScriptableObject.CreateInstance<T>();
            _container.Inject(temp);
            return temp;
        }

        /*public void InstatinatePresenter(string prefab, SceneObject model)
        {
            GameObject temp = PrefabProviderTest.Instance.GetPrefabByName(prefab);
            Presenter presenter = _container.InstantiatePrefab(temp).GetComponent<Presenter>();
            presenter.InstatiatePresenter(model);
        }

        public void InstatinatePresenter(string prefab, SceneObject model, params object[] args)
        {
            GameObject temp = PrefabProviderTest.Instance.GetPrefabByName(prefab);
            Presenter presenter = _container.InstantiatePrefab(temp).GetComponent<Presenter>();
            presenter.InstatiatePresenter(model, args);
        }*/
    }
}