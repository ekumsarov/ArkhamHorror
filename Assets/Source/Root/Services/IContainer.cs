using System.Collections.Generic;

namespace EVI
{
    public interface IContainer<T>
    {
        void Add(string id, T item);
        bool Remove(string id);
        T Get(string id);
        List<T> GetAll();
    }
}
