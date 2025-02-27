using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpriteButtonIcon : SpriteButton
{
    [SerializeField, BoxGroup("Settings")] private float moveYAmount;

    [SerializeField, Foldout("Setup")] private Transform iconTransform;

    public float BoundSize => spriteRenderer.bounds.size.x;

    protected override void NormalEnter()
    {
        base.NormalEnter();

        iconTransform.localPosition = Vector3.zero;
    }

    protected override void PressedEnter()
    {
        base.PressedEnter();

        iconTransform.localPosition = Vector3.up * -moveYAmount;
    }
}