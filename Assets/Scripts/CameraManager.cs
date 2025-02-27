using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField, Foldout("Setup")] private Camera mainCamera;

    public Camera MainCamera => mainCamera;
    public float OrtoHeight => mainCamera.orthographicSize * 2f;
    public float OrtoWidth => OrtoHeight * Screen.width / Screen.height;

    public Ray ScreenPointToRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
}