using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EVI;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using UnityEngine.Localization.Settings;
using System;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using EVI.Game;
using EVI.DDSystem;
using DG.Tweening;

public class Root : MonoBehaviour, IInitializable
{
    [Inject] private readonly Instatinator _instatinator;
    [Inject] private readonly CameraHandler _camera;
    [Inject] private readonly ContainerSystem _containers;

    //[SerializeField] private CampaignData _campaignData;
    [SerializeField, FilePath] private string _campaignPath;

    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private Transform _resourcePanel;

    public void Initialize()
    {
        /*CampaignData _campaignData = SerializationModule.DeserializeFromJsonPath<CampaignData>(_campaignPath);

        foreach(var card in _campaignData.GameCards)
        {
            GameCard gameCard = _instatinator.GetModel<GameCard>(card);
            _instatinator.InstatinateAndGetPresenter<GameCardView>(gameCard.Prefab, gameCard);
            _containers.RegisterObject(gameCard.ID, gameCard);

        }

        foreach(var loc in _campaignData.Locations)
        {
            Location location = _instatinator.GetModel<Location>(loc);
            _instatinator.InstatinateAndGetPresenter<LocationView>(location.Prefab, location);
            _containers.RegisterObject(location.ID, location);

        }

        CardCell mainCell = _instatinator.GetModel<CardCell>(_campaignData.MainLayout);
        _instatinator.InstatinateAndGetPresenter<CardCellView>(_campaignData.MainLayout.Prefab, mainCell);
        _containers.RegisterObject(mainCell.ID, mainCell);*/

        
        Resource resource = _instatinator.GetModel<Resource>(_resource);
        ResourceView resView = _instatinator.InstatinateAndGetPresenter<ResourceView>(_resourceView.gameObject, resource);
        resView.transform.SetParent(_resourcePanel);
    }
}