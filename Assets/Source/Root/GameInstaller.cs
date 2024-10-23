using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EVI.DDSystem;
using EVI.Inputs;

namespace EVI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Instatinator _instatinator;
        [SerializeField] private CameraHandler _bounds;
        [SerializeField] private Root _root;
        [SerializeField] private DragDropSystem _dragDropSystem;
        [SerializeField] private InputHandler _inputHandler;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ContainerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraHandler>().FromInstance(_bounds).AsSingle();
            Container.BindInterfacesAndSelfTo<Instatinator>().FromInstance(_instatinator).AsSingle();
            Container.BindInterfacesAndSelfTo<DragDropSystem>().FromInstance(_dragDropSystem).AsSingle();
            Container.BindInterfacesAndSelfTo<InputHandler>().FromInstance(_inputHandler).AsSingle();

            Container.BindInterfacesAndSelfTo<Root>().FromInstance(_root).AsSingle();
        }
    }
}

