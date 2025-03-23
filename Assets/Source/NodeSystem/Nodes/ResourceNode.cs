using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;
using Sirenix.OdinInspector;
using EVI.Game;

namespace EVI
{
    public partial class ResourceNode : LogicNode
    {
        [Inject] private ContainerSystem _containers;
        [Inject] private Instatinator _instatinator;

        [SerializeField] private NodeActionType _action;

        [SerializeField, ShowIf("@_action != NodeActionType.Create"), ValueDropdown("IDs")] private string _resource;
        [SerializeField, ShowIf("@_action == NodeActionType.Modify || _action == NodeActionType.Setup")] private int _amount;
        [SerializeField, ShowIf("@_action == NodeActionType.Create")] private Resource _data;
        


        #region Logic

        protected override void EnterExternal()
        {
            if(_action == NodeActionType.Create)
            {
                Resource res = _instatinator.GetModel<Resource>(_data);
                _instatinator.InstatinateAndGetPresenter<ResourceView>(_data.Prefab, res);
                
                _containers.RegisterObject<Resource>(res.ID, res);
            }
            else if(_action == NodeActionType.Remove)
            {
                _containers.RemoveObject<Resource>(_resource);
            }
            else
            {
                Resource res = _containers.GetObject<Resource>(_resource);

                if(_action == NodeActionType.Modify)
                    res.CurrentValue += _amount;
                else
                    res.CurrentValue = _amount;
            }

            Exit();
        }

        #endregion

        #region  Editor

        private IEnumerable<ValueDropdownItem<string>> IDs => AssetSelector.GetDropdownIDs<Resource>();

        #endregion
    }
}

