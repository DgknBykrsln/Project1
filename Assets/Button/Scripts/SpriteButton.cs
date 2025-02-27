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

    [SerializeField, Foldout("Setup")] private SpriteRenderer spriteRenderer;

    private StateMachine<SpriteButton, SpriteButtonState> stateMachine;

    private Camera mainCamera;

    [Inject]
    private void Construct(StateMachine<SpriteButton, SpriteButtonState> injectedStateMachine)
    {
        stateMachine = injectedStateMachine;
        stateMachine.Initialize(this);
        stateMachine.ChangeState(SpriteButtonState.Normal);
        mainCamera = Camera.main;
    }

    private bool IsMouseOver()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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