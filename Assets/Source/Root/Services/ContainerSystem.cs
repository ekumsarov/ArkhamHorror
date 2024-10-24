using System;
using System.Collections.Generic;

namespace EVI
{
    public class ContainerSystem
{
    private Dictionary<Type, object> _containers = new Dictionary<Type, object>();

    public void RegisterContainer<T>(IContainer<T> container)
    {
        _containers[typeof(T)] = container;
    }

    public void RegisterObject<T>(string id, T obj)
    {
        Type type = typeof(T);

        if (!_containers.ContainsKey(type))
        {
            // Автоматически регистрируем новый контейнер, если он не существует
            RegisterContainer(new Container<T>());
        }

        if (_containers[type] is IContainer<T> container)
        {
            container.Add(id, obj);
        }
    }

    public void RemoveObject<T>(string id)
    {
        Type type = typeof(T);

        if (!_containers.ContainsKey(type))
        {
            // Автоматически регистрируем новый контейнер, если он не существует
            RegisterContainer(new Container<T>());
        }

        if (_containers[type] is IContainer<T> container)
        {
            container.Remove(id);
        }
    }

    public IContainer<T> GetContainer<T>()
    {
        Type type = typeof(T);
        
        if (_containers.TryGetValue(type, out var container) && container is IContainer<T> typedContainer)
        {
            return typedContainer;
        }

        throw new Exception($"Контейнер для типа {type.Name} не найден.");
    }

    public T GetObject<T>(string id)
    {
        return GetContainer<T>().Get(id);
    }

    public List<T> GetAllObjects<T>()
    {
        return GetContainer<T>().GetAll();
    }
}

}