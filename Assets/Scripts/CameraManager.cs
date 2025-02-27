using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    [SerializeField, Foldout("Setup")] private Camera mainCamera;

    public float Height => mainCamera.orthographicSize * 2f;
    public float Width => Height * Screen.width / Screen.height;

    public Ray ScreenPointToRay() => mainCamera.ScreenPointToRay(Input.mousePosition);

    private Vector2Int screenSize;


    [Inject]
    private void Construct()
    {
        screenSize = new Vector2Int(Screen.width, Screen.height);
    }

    public Vector3 GetScreenTopPosition()
    {
        var screenTopCenter = new Vector3(screenSize.x / 2f, screenSize.y, mainCamera.nearClipPlane);
        var worldPosition = mainCamera.ScreenToWorldPoint(screenTopCenter);
        return worldPosition;
    }
}