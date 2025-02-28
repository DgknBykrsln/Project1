using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpriteButtonContent : SpriteButton
{
    [SerializeField, BoxGroup("Settings")] private float moveYAmount;

    [SerializeField, Foldout("Setup")] private Transform moveRoot;

    protected override void NormalEnter()
    {
        base.NormalEnter();

        moveRoot.localPosition = Vector3.zero;
    }

    protected override void PressedEnter()
    {
        base.PressedEnter();

        moveRoot.localPosition = Vector3.up * -moveYAmount;
    }
}