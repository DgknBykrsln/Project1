using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class ThemeManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Theme> themes;

    public Color ButtonUnPressedColor => currentTheme.ButtonUnPressedColor;
    public Color ButtonPressedColor => currentTheme.ButtonPressedColor;
    public Color BgColor => currentTheme.BgColor;

    public static UnityAction OnThemeChange;

    private Theme currentTheme => themes[currentThemeIndex];


    private int currentThemeIndex;

    [Inject]
    private void Construct(List<Theme> _themes)
    {
        themes = _themes;
        currentThemeIndex = 0;
        GridManager.OnGridUpdate += UpdateTheme;
    }

    private void OnDestroy()
    {
        GridManager.OnGridUpdate -= UpdateTheme;
    }

    private void UpdateTheme(int gridSize)
    {
        currentThemeIndex = gridSize % themes.Count;
        OnThemeChange?.Invoke();
    }
}