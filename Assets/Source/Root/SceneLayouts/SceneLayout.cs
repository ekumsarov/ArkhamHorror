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
            Grid
        }

        private enum LayoutAnchor
        {
            Center,
            Left,
            Right,
            Top,
            Bottom,
            Evenly
        }

        [OdinSerialize, ReadOnly] private SpriteRenderer _boundSprite;
        [OdinSerialize, ReadOnly] private List<SceneLayoutElement> _childs = new List<SceneLayoutElement>();

        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _leftMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _rightMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _topMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _bottomMargin;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _xSpace;
        [OdinSerialize, OnValueChanged("RebuildLayout"), FoldoutGroup("Margins")] private float _ySpace;

        [OdinSerialize, OnValueChanged("RebuildLayout")] private LayoutAnchor _anchor = LayoutAnchor.Left;
        [OdinSerialize, OnValueChanged("RebuildLayout")] private Layout _layout = Layout.Horizontal;
        [OdinSerialize, OnValueChanged("RebuildLayout"), ShowIf("_layout", Layout.Grid)] private Vector2 _cellSize = new Vector2(1, 1);
        [OdinSerialize, OnValueChanged("RebuildLayout"), ShowIf("_layout", Layout.Grid)] private Vector2 _spacing = new Vector2(0, 0);

        private void OnEnable()
        {
            Rebuild();
        }

        [Button]
        public void Rebuild()
        {
            CheckSprite();
            _childs.Clear();

            _childs = transform.Cast<Transform>()
                .Where(child => child.TryGetComponent(out SceneLayoutElement element))
                .Select(child => child.GetComponent<SceneLayoutElement>())
                .ToList();

            RebuildLayout();
        }

        private void RebuildLayout()
        {
            CheckSprite();
            switch (_layout)
            {
                case Layout.Horizontal:
                    RebuildHorizontalLayout();
                    break;
                case Layout.Vertical:
                    RebuildVerticalLayout();
                    break;
                case Layout.Grid:
                    RebuildGridLayout();
                    break;
            }
        }

        private void RebuildHorizontalLayout()
        {
            Vector2 nextPosition = Vector2.zero;

            switch (_anchor)
            {
                case LayoutAnchor.Left:
                    nextPosition = new Vector2(_boundSprite.bounds.min.x + _leftMargin, _boundSprite.bounds.center.y);
                    break;
                case LayoutAnchor.Right:
                    nextPosition = new Vector2(_boundSprite.bounds.max.x - _rightMargin, _boundSprite.bounds.center.y);
                    break;
                case LayoutAnchor.Center:
                    float totalWidth = _childs.Sum(child => child.SBounds.size.x) + (_xSpace * (_childs.Count - 1));
                    nextPosition = new Vector2(_boundSprite.bounds.center.x - totalWidth / 2, _boundSprite.bounds.center.y);
                    break;
                case LayoutAnchor.Evenly:
                    _xSpace = (_boundSprite.bounds.size.x - _leftMargin - _rightMargin - _childs.Sum(c => c.SBounds.size.x)) / (_childs.Count + 1);
                    nextPosition = new Vector2(_boundSprite.bounds.min.x + _leftMargin + _xSpace, _boundSprite.bounds.center.y);
                    break;
            }

            foreach (var child in _childs)
            {
                if (!child.isActiveAndEnabled) continue;

                float halfWidth = child.SBounds.size.x / 2;
                child.transform.position = new Vector2(nextPosition.x + halfWidth, nextPosition.y);
                nextPosition.x += halfWidth + _xSpace + halfWidth;
            }
        }

        private void RebuildVerticalLayout()
        {
            Vector2 nextPosition = Vector2.zero;

            switch (_anchor)
            {
                case LayoutAnchor.Top:
                    nextPosition = new Vector2(_boundSprite.bounds.center.x, _boundSprite.bounds.max.y - _topMargin);
                    break;
                case LayoutAnchor.Bottom:
                    nextPosition = new Vector2(_boundSprite.bounds.center.x, _boundSprite.bounds.min.y + _bottomMargin);
                    break;
                case LayoutAnchor.Center:
                    float totalHeight = _childs.Sum(child => child.SBounds.size.y) + (_ySpace * (_childs.Count - 1));
                    nextPosition = new Vector2(_boundSprite.bounds.center.x, _boundSprite.bounds.center.y + totalHeight / 2);
                    break;
                case LayoutAnchor.Evenly:
                    _ySpace = (_boundSprite.bounds.size.y - _topMargin - _bottomMargin - _childs.Sum(c => c.SBounds.size.y)) / (_childs.Count + 1);
                    nextPosition = new Vector2(_boundSprite.bounds.center.x, _boundSprite.bounds.max.y - _topMargin - _ySpace);
                    break;
            }

            foreach (var child in _childs)
            {
                if (!child.isActiveAndEnabled) continue;

                float halfHeight = child.SBounds.size.y / 2;
                child.transform.position = new Vector2(nextPosition.x, nextPosition.y - halfHeight);
                nextPosition.y -= halfHeight + _ySpace + halfHeight;
            }
        }

        private void RebuildGridLayout()
        {
            Vector2 startPosition = new Vector2(_boundSprite.bounds.min.x + _leftMargin, _boundSprite.bounds.max.y - _topMargin);
            Vector2 currentPosition = startPosition;

            foreach (var child in _childs)
            {
                if (!child.isActiveAndEnabled)
                    continue;

                child.transform.position = currentPosition;
                currentPosition.x += _cellSize.x + _spacing.x;

                if (currentPosition.x + _cellSize.x > _boundSprite.bounds.max.x - _rightMargin)
                {
                    currentPosition.x = startPosition.x;
                    currentPosition.y -= _cellSize.y + _spacing.y;
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
