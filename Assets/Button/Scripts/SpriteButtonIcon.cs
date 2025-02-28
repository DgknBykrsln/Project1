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

    public static float ScaleDuration => 0.1f;

    private Color successColor => Color.green;

    private Tween colorTween;
    private Tween fadeTween;
    private Tween scaleTween;

    private bool canClick = true;

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

    protected override void PressedExecute()
    {
        if (!canClick) return;

        base.PressedExecute();
    }

    public void OpenIcon()
    {
        scaleTween?.Kill();
        scaleTween = scaleRoot.DOScale(Vector3.one, ScaleDuration).SetEase(Ease.OutBack);

        fadeTween?.Kill();
        fadeTween = iconSpriteRenderer.DOFade(1f, ScaleDuration).SetEase(Ease.OutSine);
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
        fadeTween = iconSpriteRenderer.DOFade(0f, ScaleDuration).SetEase(Ease.InSine);

        scaleTween?.Kill();
        scaleTween = scaleRoot.DOScale(Vector3.zero, ScaleDuration).SetEase(Ease.InBack);
    }

    public void DoSuccessAnimation(float duration)
    {
        colorTween?.Kill();
        colorTween = spriteRenderer.DOColor(successColor, duration).SetLoops(2, LoopType.Yoyo);
    }

    public void DeactivateClick()
    {
        canClick = false;
    }

    public void ActivateClick()
    {
        canClick = true;
    }
}