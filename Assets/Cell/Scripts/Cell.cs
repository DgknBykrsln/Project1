using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Cell : Poolable
{
    [SerializeField, Foldout("Setup")] private SpriteButtonIcon spriteButtonIcon;

    public float BoundSize => boundSize;

    private float boundSize;

    [Inject]
    private void Construct()
    {
        boundSize = spriteButtonIcon.BoundSize;
    }

    public void PrepareCell(Transform parent, Vector3 position, Vector3 scale, string name)
    {
        Transform transform1;
        (transform1 = transform).SetParent(parent);
        transform1.position = position;
        transform1.localScale = scale;
        transform1.name = name;
    }
}