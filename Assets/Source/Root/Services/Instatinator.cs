using UnityEngine;
using Zenject;
using System;

namespace EVI
{
    public class Instatinator : MonoBehaviour
    {
        [Inject] DiContainer _container;

        public T GetModel<T>()
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(null);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1>(Tparam1 tparam1)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(null, tparam1);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1, Tparam2>(Tparam1 tparam1, Tparam2 param2)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(null, tparam1, param2);
            _container.QueueForInject(temp);

            return temp;
        }

        public T GetModel<T, Tparam1, Tparam2, Tparam3>(Tparam1 tparam1, Tparam2 param2, Tparam3 param3)
        {
            T temp = _container.Instantiate<T>();
            BaseModel obj = temp as BaseModel;
            if (obj == null)
            {
                throw new ArgumentException("Cannot find such model" + temp.ToString());
            }

            obj.Init(null, tparam1, param2, param3);
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

        public void InstatinatePresenter(GameObject prefab, BaseModel model, params object[] args)
        {
            BaseView presenter = _container.InstantiatePrefab(prefab).GetComponent<BaseView>();
            presenter.Init(model, args);
        }

        public T InstatinatePrefab<T>(GameObject prefab)
        {
            return _container.InstantiatePrefab(prefab).GetComponent<T>();
        }

        public void QueueForInject(object forInject)
        {
            _container.Inject(forInject);
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