using SimpleJSON;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EVI.Game
{
    [JSONSerializable, CreateAssetMenu(menuName = "Models/Location")]
    public class Location : TypedModel<LocationView>
    {
        [Inject] private Instatinator _instatinator;
        [Inject] private readonly ContainerSystem _containers;

        [SerializeField, JSONConvert] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField, OnInspectorInit("UpdateComponents"), JSONConvert] private List<CardCell> _cells;
        private List<CardCell> _actualCells;
        public List<CardCell> Cells => _actualCells;

        private void UpdateComponents()
        {
            if(_cells == null)
                _cells = new List<CardCell>();
        }

        protected override void InitializeBaseExternal()
        {
            base.InitializeBaseExternal();

            if(_actualCells == null)
                _actualCells = new List<CardCell>();

            foreach(var cell in _cells)
            {
                CardCell actualCell = _instatinator.GetModel<CardCell>(cell);
                _actualCells.Add(actualCell);
                _instatinator.InstatinateAndGetPresenter<CardCellView>(actualCell.Prefab, actualCell);
                _containers.RegisterObject(actualCell.ID, actualCell);
            }
        }

        public List<GameCard> GetCard(CardType cardType)
        {
            return null;
        }

#if UNITY_EDITOR
        [Button]
        private void AddCell()
        {
            if(_cells == null)
                _cells = new List<CardCell>();

            CardCell cell = ScriptableObject.CreateInstance<CardCell>();
            cell.name = this.ID + "Cell" + _cells.Count.ToString();
            _cells.Add(cell);
            AssetDatabase.AddObjectToAsset(cell, this);
            AssetDatabase.SaveAssets();
            ActualizeLocation();
        }

        [Button]
        private void ActualizeLocation()
        {
            if(_cells == null)
                _cells = new List<CardCell>();

            List<UnityEngine.Object> tempCommands = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this)).ToList();


            int index = 0;
            foreach (var obj in tempCommands)
            {
                if (obj is CardCell)
                {
                    if (_cells.Any(cmd => cmd == (obj as CardCell)) == false)
                    {
                        obj.name = this.ID + "Cell" + index;
                        AssetDatabase.RemoveObjectFromAsset(obj);
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();
                        index++;
                    }
                }
            }
        }
#endif
    }
}
