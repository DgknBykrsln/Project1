using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

public class UiPanel : MonoBehaviour
{
    [SerializeField, Foldout("Setup")] private TextMeshPro matchText;

    [SerializeField] private List<SpriteRenderer> spriteRenderers;

    private GridManager gridManager;
    private ThemeManager themeManager;
    private CameraManager cameraManager;
    private CustomInputField customInputField;

    private int matchCount;

    [Inject]
    private void Construct(GridManager _gridManager, ThemeManager _themeManager, CameraManager _cameraManager, CustomInputField _customInputField)
    {
        customInputField = _customInputField;
        cameraManager = _cameraManager;
        gridManager = _gridManager;
        themeManager = _themeManager;
        GridManager.CellsMatched += OnGridUpdate;
        ThemeManager.OnThemeChange += ChangeBgColors;
        ChangeBgColors();
        UpdateMatchText();

        transform.position = cameraManager.GetScreenBottomPosition() + Vector3.up * 2f;
    }

    private void OnDestroy()
    {
        ThemeManager.OnThemeChange -= ChangeBgColors;
        GridManager.CellsMatched -= OnGridUpdate;
    }

    private void ChangeBgColors()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = themeManager.ButtonUnPressedColor;
        }
    }

    private void OnGridUpdate()
    {
        matchCount++;
        UpdateMatchText();
    }

    private void UpdateMatchText()
    {
        matchText.text = $"Match Count: {matchCount}";
    }

    public void Rebuild()
    {
        gridManager.UpdateGrid(customInputField.SelectedText);
        matchCount = 0;
        UpdateMatchText();
    }
}