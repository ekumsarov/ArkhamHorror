using System.Collections.Generic;
using System.Linq;

namespace EVI
{
    public class Container<T> : IContainer<T>
    {
        private Dictionary<string, T> _items = new Dictionary<string, T>();

        public void Add(string id, T item)
        {
            if (!_items.ContainsKey(id))
            {
                _items[id] = item;
            }
        }

        public bool Remove(string id)
        {
            return _items.Remove(id);
        }

        public T Get(string id)
        {
            return _items.TryGetValue(id, out var item) ? item : default;
        }

        public List<T> GetAll()
        {
            return _items.Values.ToList();
        }
    }

}