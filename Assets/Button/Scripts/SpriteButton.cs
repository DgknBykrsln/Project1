using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class SpriteButton : MonoBehaviour
{
    public enum SpriteButtonState
    {
        Normal,
        Pressed
    }

    [SerializeField, BoxGroup("Settings")] private UnityEvent onClick;
    [SerializeField, BoxGroup("Settings")] private Sprite normalSprite;
    [SerializeField, BoxGroup("Settings")] private Sprite pressedSprite;

    [SerializeField, Foldout("Setup")] protected SpriteRenderer spriteRenderer;

    private StateMachine<SpriteButton, SpriteButtonState> stateMachine;

    private CameraManager cameraManager;
    private ThemeManager themeManager;

    [Inject]
    private void Construct(CameraManager _cameraManager, StateMachine<SpriteButton, SpriteButtonState> _stateMachine, ThemeManager _themeManager)
    {
        cameraManager = _cameraManager;
        stateMachine = _stateMachine;
        themeManager = _themeManager;
        stateMachine.Initialize(this);
        stateMachine.ChangeState(SpriteButtonState.Normal);
        ThemeManager.OnThemeChange += ChangeButtonColor;
    }

    private void OnDestroy()
    {
        ThemeManager.OnThemeChange -= ChangeButtonColor;
    }

    private void ChangeButtonColor()
    {
        spriteRenderer.color = stateMachine.CurrentState == SpriteButtonState.Normal ? themeManager.ButtonUnPressedColor : themeManager.ButtonPressedColor;
    }

    private bool IsMouseOver()
    {
        var ray = cameraManager.ScreenPointToRay();
        var hit = Physics2D.Raycast(ray.origin, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    private void Update()
    {
        stateMachine.Execute();
    }

    #region Normal

    protected virtual void NormalEnter()
    {
        spriteRenderer.sprite = normalSprite;
        spriteRenderer.color = themeManager.ButtonUnPressedColor;
    }

    private void NormalExecute()
    {
        if (Input.GetMouseButtonDown(0) && IsMouseOver())
        {
            stateMachine.ChangeState(SpriteButtonState.Pressed);
        }
    }

    private void NormalExit()
    {
    }

    #endregion


    #region Pressed

    protected virtual void PressedEnter()
    {
        spriteRenderer.sprite = pressedSprite;
        spriteRenderer.color = themeManager.ButtonPressedColor;
    }

    private void PressedExecute()
    {
        if (Input.GetMouseButtonUp(0))
        {
            stateMachine.ChangeState(SpriteButtonState.Normal);

            if (IsMouseOver())
            {
                onClick?.Invoke();
            }
        }
    }

    private void PressedExit()
    {
    }

    #endregion
}