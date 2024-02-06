using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Linq;

namespace EVI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SceneLayout : SerializedMonoBehaviour
    {
        private enum Layout
        {
            Vertical,
            Horizontal,

        }

        private enum LayotAnchor
        {
            Center,
            Left,
            Right
        }

        [OdinSerialize, ReadOnly] private SpriteRenderer _boundSprite;
        [OdinSerialize, ReadOnly] private List<SceneLayoutElement> _childs = new List<SceneLayoutElement>();
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _leftMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _rightMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _topMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _bottomMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _xSpace;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _ySpace;
        [OdinSerialize, OnValueChanged("RebuildLayout")] private LayotAnchor _anchor = LayotAnchor.Left;
        [OdinSerialize, OnValueChanged("RebuildLayout")] private Layout _layout = Layout.Horizontal;

        private void OnEnable()
        {
            Rebuild();
        }

        [Button]
        private void Rebuild()
        {
            CheckSprite();
            _childs.Clear();
            _childs = GetComponentsInChildren<SceneLayoutElement>().ToList();
            RebuildLayout();
        }

        private void RebuildLayout()
        {
            CheckSprite();
            if (_layout == Layout.Horizontal)
            {
                Vector2 nextPosition = new Vector2();
                if (_anchor == LayotAnchor.Left)
                {
                    nextPosition = new Vector2(_boundSprite.bounds.Left() + _leftMargin, _boundSprite.bounds.center.y);
                    for (int i = 0; i < _childs.Count; i++)
                    {
                        if (_childs[i].isActiveAndEnabled == false)
                            continue;

                        _childs[i].transform.position = new Vector2(nextPosition.x + _childs[i].SBounds.size.x/2, nextPosition.y);
                        nextPosition = new Vector2(_childs[i].transform.position.x + _childs[i].SBounds.size.x / 2 + _xSpace, nextPosition.y);
                    }
                }
                else if(_anchor == LayotAnchor.Right)
                {
                    nextPosition = new Vector2(_boundSprite.bounds.Right() - _rightMargin, _boundSprite.bounds.center.y);
                    for (int i = 0; i < _childs.Count; i++)
                    {
                        if (_childs[i].isActiveAndEnabled == false)
                            continue;

                        _childs[i].transform.position = new Vector2(nextPosition.x - _childs[i].SBounds.size.x / 2, nextPosition.y);
                        nextPosition = new Vector2(_childs[i].transform.position.x - _childs[i].SBounds.size.x / 2 - _xSpace, nextPosition.y);
                    }
                }
            }
        }

        private void CheckSprite()
        {
            if (_boundSprite == null)
                _boundSprite = GetComponent<SpriteRenderer>();
        }
    }
}

