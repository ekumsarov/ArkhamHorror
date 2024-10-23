using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EVI.Game
{
    public class LocationView : TypedView<Location>
    {
        [SerializeField, ReadOnly, OnInspectorInit("UpdateLocation")] private SceneLayout _sceneLayout;
        [SerializeField, OnInspectorInit("UpdateLocation")] private List<CardCellView> _cells;

        protected override void InitializeBaseInternal()
        {
            base.InitializeBaseInternal();

            if(_cells == null)
                _cells = new List<CardCellView>();

            _cells.Clear();

            foreach(var cell in Model.Cells)
            {
                cell.View.transform.SetParent(transform);
                _cells.Add(cell.View);
            }

            _sceneLayout.Rebuild();

            foreach(var cell in _cells)
            {
                cell.RebuildLayout();
            }
        }

        private void UpdateLocation()
        {
            _cells = GetComponentsInChildren<CardCellView>().ToList();

            if(_sceneLayout == null)
                _sceneLayout = GetComponent<SceneLayout>();
        }
    }
}

