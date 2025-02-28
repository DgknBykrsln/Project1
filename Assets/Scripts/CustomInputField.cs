using NaughtyAttributes;
using UnityEngine;
using TMPro;
using Zenject;

public class CustomInputField : MonoBehaviour
{
    [SerializeField, Foldout("Setup")] private TextMeshPro text;
    [SerializeField, Foldout("Setup")] private LineRenderer cursorLine;

    public int SelectedText => int.Parse(inputText);

    private string inputText = "";
    private bool isActive;

    private CameraManager cameraManager;
    private GridManager gridManager;

    [Inject]
    private void Construct(CameraManager _cameraManager, GridManager _gridManager)
    {
        gridManager = _gridManager;
        cameraManager = _cameraManager;
        InitializeText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isActive = IsMouseOver();
            if (isActive)
            {
                inputText = "";
                text.text = "";
                cursorLine.enabled = true;
                UpdateCursorPosition();
            }
            else
            {
                cursorLine.enabled = false;
                if (string.IsNullOrEmpty(inputText))
                {
                    ResetToGridSize();
                }
            }
        }

        if (isActive)
        {
            var textChanged = false;
            foreach (var c in Input.inputString)
            {
                if (c == '\b')
                {
                    if (inputText.Length > 0)
                    {
                        inputText = inputText[..^1];
                        textChanged = true;
                    }
                }
                else if (c is '\n' or '\r')
                {
                    isActive = false;
                    cursorLine.enabled = false;
                    if (string.IsNullOrEmpty(inputText))
                    {
                        ResetToGridSize();
                    }
                }
                else if (char.IsDigit(c) && inputText.Length < 4)
                {
                    inputText += c;
                    textChanged = true;
                }
            }

            if (textChanged)
            {
                text.text = inputText;
                UpdateCursorPosition();
            }
        }
    }

    private void InitializeText()
    {
        inputText = gridManager.GridSize.ToString();
        text.text = inputText;
        UpdateCursorPosition();
    }

    private bool IsMouseOver()
    {
        var ray = cameraManager.ScreenPointToRay();
        var hit = Physics2D.Raycast(ray.origin, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }


    private void ResetToGridSize()
    {
        inputText = gridManager.GridSize.ToString();
        text.text = inputText;
    }

    private void UpdateCursorPosition()
    {
        if (!isActive)
        {
            cursorLine.enabled = false;
            return;
        }

        text.ForceMeshUpdate();

        if (text.textInfo.characterCount > 0)
        {
            var lastCharInfo = text.textInfo.characterInfo[text.textInfo.characterCount - 1];
            var cursorStartPos = lastCharInfo.topRight + new Vector3(0.05f, 0, 0);
            var cursorEndPos = cursorStartPos + new Vector3(0, -0.2f, 0);

            cursorLine.SetPosition(0, text.transform.TransformPoint(cursorStartPos));
            cursorLine.SetPosition(1, text.transform.TransformPoint(cursorEndPos));
        }
        else
        {
            var textBounds = text.textBounds;
            var cursorStartPos = new Vector3(textBounds.min.x, textBounds.max.y, 0) + new Vector3(0.05f, 0, 0);
            var cursorEndPos = cursorStartPos + new Vector3(0, -0.2f, 0);

            cursorLine.SetPosition(0, text.transform.TransformPoint(cursorStartPos));
            cursorLine.SetPosition(1, text.transform.TransformPoint(cursorEndPos));
        }
    }
}