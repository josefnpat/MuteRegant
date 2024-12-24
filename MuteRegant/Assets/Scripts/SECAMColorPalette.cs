using System.Collections.Generic;
using UnityEngine;

public class SECAMColorPalette
{
    private static readonly List<Color32> _colors = new List<Color32>
    {
        new Color32(0x00, 0x00, 0x00, 0xFF),
        new Color32(0x00, 0x27, 0xFB, 0xFF),
        new Color32(0xFF, 0x30, 0x16, 0xFF),
        new Color32(0xFF, 0x3F, 0xFC, 0xFF),
        new Color32(0x00, 0xF9, 0x2C, 0xFF),
        new Color32(0x00, 0xFC, 0xFE, 0xFF),
        new Color32(0xFF, 0xFD, 0x33, 0xFF),
        new Color32(0xFF, 0xFF, 0xFF, 0xFF),
    };

    public enum Enum
    {
        black, blue, red, magenta, green, cyan, yellow, white
    }

    public static Color GetColor(Enum color)
    {
        return _colors[(int)color];
    }

    public static string FormatColorString(Enum color, string text)
    {
        string hex = ColorUtility.ToHtmlStringRGB(GetColor(color)).ToUpper();
        return $"<color=#{hex}>{text}</color>";
    }

}
