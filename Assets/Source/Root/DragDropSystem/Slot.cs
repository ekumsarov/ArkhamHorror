using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace EVI.DDSystem
{
public class Slot : MonoBehaviour
    {
        [SerializeField, OnInspectorInit("UpdateComponents")] private SpriteRenderer _sprite;
        [SerializeField] private bool _allowMultimple = false;
        public bool AllowMultipleItems => _allowMultimple;
        private List<BaseView> _items = new List<BaseView>();

        public Bounds Bounds => _sprite.bounds;


        public bool TryPlaceItem(BaseView item)
        {
            if(ItemPlaced == null)
                return false;

            return ItemPlaced.Invoke(item);
        }

        public void CoveredByItem(BaseView item)
        {
            if(ItemCovered == null)
                return;

            ItemCovered.Invoke(item);
        }

        public void RemoveItem(BaseView item)
        {
            if(ItemRemoved == null)
                return;

            ItemRemoved.Invoke(item);
        }

        private void UpdateComponents()
        {
            if(_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();
        }

        public event BoolTypedDelefate<BaseView> ItemPlaced;
        public event Action<BaseView> ItemRemoved;
        public event Action<BaseView> ItemCovered;
    }
}