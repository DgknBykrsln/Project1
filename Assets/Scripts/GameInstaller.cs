using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<StateMachine<SpriteButton, SpriteButton.SpriteButtonState>>().AsTransient();
    }
}