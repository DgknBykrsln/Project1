using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Cell : Poolable
{
    [SerializeField, Foldout("Setup")] private SpriteButtonIcon spriteButtonIcon;

    public enum CellState
    {
        Inactive,
        Active
    }

    public CellState State => stateMachine.CurrentState;

    public float BoundSize => boundSize;

    private float boundSize;

    private StateMachine<Cell, CellState> stateMachine;

    private GridManager gridManager;

    [Inject]
    private void Construct(StateMachine<Cell, CellState> _stateMachine, GridManager _gridManager)
    {
        gridManager = _gridManager;
        stateMachine = _stateMachine;
        stateMachine.Initialize(this);
        boundSize = spriteButtonIcon.BoundSize;
        spriteButtonIcon.CloseIcon(true);
    }

    private void Update()
    {
        stateMachine.Execute();
    }

    #region Inactive

    protected virtual void InactiveEnter()
    {
        spriteButtonIcon.CloseIcon();
    }

    private void InactiveExecute()
    {
    }

    private void InactiveExit()
    {
    }

    #endregion

    #region Active

    protected virtual void ActiveEnter()
    {
        spriteButtonIcon.OpenIcon();
    }

    private void ActiveExecute()
    {
    }

    private void ActiveExit()
    {
    }

    #endregion

    #region Methods

    public void PrepareCell(Transform parent, Vector3 position, Vector3 scale, string name)
    {
        stateMachine.ChangeState(CellState.Inactive);
        transform.SetParent(parent);
        transform.position = position;
        transform.localScale = scale;
        transform.name = name;
    }

    public void ToggleState()
    {
        var currentState = stateMachine.CurrentState;
        stateMachine.ChangeState(currentState == CellState.Inactive ? CellState.Active : CellState.Inactive);
    }

    public void ToggleStateAndNotify()
    {
        ToggleState();
        gridManager.NotifyStateChange();
    }

    #endregion
}