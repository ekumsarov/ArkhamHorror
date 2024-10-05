using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EVI;
using EVI.Inputs;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using UnityEngine.Localization.Settings;
using System;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class Root : MonoBehaviour, IInitializable
{
    [Inject] private readonly Instatinator _instatinator;
    [Inject] private readonly CameraHandler _camera;

    private int count = 0;

    public void Init()
    {
    }
    public void Initialize()
    {
        Init();
    }
}