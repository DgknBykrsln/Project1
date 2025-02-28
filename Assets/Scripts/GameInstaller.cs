using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField, Foldout("Setup Game")] private ObjectPooler objectPooler;
    [SerializeField, Foldout("Setup Game")] private CameraManager cameraManager;
    [SerializeField, Foldout("Setup Game")] private GridManager gridManager;

    [SerializeField, Foldout("Setup Theme")] private ThemeManager themeManager;
    [SerializeField, Foldout("Setup Theme")] private List<Theme> themes;

    public override void InstallBindings()
    {
        BindStateMachine<Cell, Cell.CellState>();
        BindStateMachine<SpriteButton, SpriteButton.SpriteButtonState>();

        BindThemeManager();

        BindFromInstance(cameraManager);
        BindFromInstance(gridManager);
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

    private void BindThemeManager()
    {
        Container.BindInstance(themes).AsSingle();
        Container.BindInstance(themeManager).AsSingle().WithArguments(themes);
    }
}