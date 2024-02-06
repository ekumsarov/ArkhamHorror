using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EVI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CameraHandler _bounds;
        [SerializeField] private Root _root;

        public override void InstallBindings()
        {
            Container.Bind<CameraHandler>().FromInstance(_bounds).AsSingle();

            Container.Bind<Root>().FromInstance(_root).AsSingle();
        }
    }
}

