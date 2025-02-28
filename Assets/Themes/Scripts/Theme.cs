using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Game/Theme")]
public class Theme : ScriptableObject
{
    [SerializeField, BoxGroup("Settings")] private Color buttonUnPressedColor;
    [SerializeField, BoxGroup("Settings")] private Color buttonPressedColor;
    [SerializeField, BoxGroup("Settings")] private Color bgColor;

    public Color ButtonUnPressedColor => buttonUnPressedColor;
    public Color ButtonPressedColor => buttonPressedColor;
    public Color BgColor => bgColor;
}