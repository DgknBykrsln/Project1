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
    [SerializeField, Foldout("Setup")] private GridManager gridManager;

    public override void InstallBindings()
    {
        BindStateMachine<Cell, Cell.CellState>();
        BindStateMachine<SpriteButton, SpriteButton.SpriteButtonState>();

        BindFromInstance(gridManager);
        BindFromInstance(cameraManager);
        BindFromInstance(objectPooler);
    }

    private void BindStateMachine<T, TState>() where T : MonoBehaviour where TState : struct, Enum
    {
        Container.Bind<StateMachine<T, TState>>().AsTransient();
    }

    private void BindFromInstance<T>(T instance) where T : class
    {
        Container.Bind<T>().FromInstance(instance).AsSingle();
    }
}