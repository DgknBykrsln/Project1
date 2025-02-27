using NaughtyAttributes;
using UnityEngine;

public class SpriteFitter : MonoBehaviour
{
    [SerializeField, Foldout("Setup")] private SpriteRenderer spriteRenderer;

    private Camera mainCamera;
    
    

    private void Start()
    {
        mainCamera = Camera.main;
        FitSpriteToCamera();
    }

    private void FitSpriteToCamera()
    {
        var orthoHeight = mainCamera.orthographicSize * 2f;
        var orthoWidth = orthoHeight * Screen.width / Screen.height;

        var bounds = spriteRenderer.sprite.bounds;
        var spriteHeight = bounds.size.y;
        var spriteWidth = bounds.size.x;

        var scaleX = orthoWidth / spriteWidth;
        var scaleY = orthoHeight / spriteHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1);

        var targetYPoz = scaleY / 2;
        transform.position = Vector3.up * targetYPoz;
    }
}