using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace EVI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SceneLayoutElement : SerializedMonoBehaviour
    {
        [OdinSerialize, OnInspectorInit("CheckSprite"), ReadOnly] private SpriteRenderer _sprite;
        private void CheckSprite()
        {
            if (_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();

            if (_sprite == null)
                gameObject.SetActive(false);
        }


        public Bounds SBounds => _sprite.bounds;
    }
}

