using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class SpriteButtonIcon : SpriteButton
{
    [SerializeField, BoxGroup("Settings")] private float moveYAmount;

    [SerializeField, Foldout("Setup")] private Transform moveRoot;
    [SerializeField, Foldout("Setup")] private Transform scaleRoot;
    [SerializeField, Foldout("Setup")] private SpriteRenderer iconSpriteRenderer;

    public float BoundSize => spriteRenderer.bounds.size.x;

    private static float scaleDuration => 0.1f;

    private Tween fadeTween;
    private Tween scaleTween;

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

    public void OpenIcon()
    {
        scaleTween?.Kill();
        scaleTween = scaleRoot.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutBack);

        fadeTween?.Kill();
        fadeTween = iconSpriteRenderer.DOFade(1f, scaleDuration).SetEase(Ease.OutSine);
    }

    public void CloseIcon(bool isInstant = false)
    {
        if (isInstant)
        {
            var color = iconSpriteRenderer.color;
            color.a = 0f;
            iconSpriteRenderer.color = color;

            scaleRoot.localScale = Vector3.zero;
            return;
        }

        fadeTween?.Kill();
        fadeTween = iconSpriteRenderer.DOFade(0f, scaleDuration).SetEase(Ease.InSine);

        scaleTween?.Kill();
        scaleTween = scaleRoot.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.InBack);
    }
}