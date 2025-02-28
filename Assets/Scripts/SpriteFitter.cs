using System;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class SpriteFitter : MonoBehaviour
{
    [SerializeField, Foldout("Setup")] private SpriteRenderer spriteRenderer;

    private CameraManager cameraManager;
    private ThemeManager themeManager;

    [Inject]
    private void Construct(CameraManager _cameraManager, ThemeManager _themeManager)
    {
        cameraManager = _cameraManager;
        themeManager = _themeManager;
        ThemeManager.OnThemeChange += ChangeBgColor;
        ChangeBgColor();
        FitSpriteToCamera();
    }

    private void OnDestroy()
    {
        ThemeManager.OnThemeChange -= ChangeBgColor;
    }

    private void ChangeBgColor()
    {
        spriteRenderer.color = themeManager.BgColor;
    }

    private void FitSpriteToCamera()
    {
        var orthoHeight = cameraManager.Height;
        var orthoWidth = cameraManager.Width;

        var bounds = spriteRenderer.sprite.bounds;
        var spriteHeight = bounds.size.y;
        var spriteWidth = bounds.size.x;

        var scaleX = orthoWidth / spriteWidth;
        var scaleY = orthoHeight / spriteHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1);

        var targetYPoz = scaleY / 2;
        transform.position = Vector3.up * targetYPoz;
    }
}