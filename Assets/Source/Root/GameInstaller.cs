using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EVI.Inputs;

namespace EVI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Instatinator _instatinator;
        [SerializeField] private CameraHandler _bounds;
        [SerializeField] private Root _root;

        public override void InstallBindings()
        {
            Container.Bind<MainInput>().AsSingle();
            Container.Bind<CameraHandler>().FromInstance(_bounds).AsSingle();
            Container.Bind<Instatinator>().FromInstance(_instatinator).AsSingle();

            Container.Bind<Root>().FromInstance(_root).AsSingle();
        }
    }
}

