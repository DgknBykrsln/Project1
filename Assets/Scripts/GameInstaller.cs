using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField, Foldout("Setup")] private ObjectPooler objectPooler;
    [SerializeField, Foldout("Setup")] private CameraManager cameraManager;

    public override void InstallBindings()
    {
        Container.Bind<StateMachine<SpriteButton, SpriteButton.SpriteButtonState>>().AsTransient();

        Container.Bind<CameraManager>().FromInstance(cameraManager).AsSingle();
        Container.Bind<ObjectPooler>().FromInstance(objectPooler).AsSingle();
    }
}