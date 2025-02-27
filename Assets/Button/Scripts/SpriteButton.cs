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

    [Inject]
    private void Construct(CameraManager _cameraManager, StateMachine<SpriteButton, SpriteButtonState> _stateMachine)
    {
        cameraManager = _cameraManager;
        stateMachine = _stateMachine;
        stateMachine.Initialize(this);
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