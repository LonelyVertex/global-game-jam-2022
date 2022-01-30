using UnityEngine;

public enum PlayerType
{
    Dark = 0,
    Light = 1
}

public static class PlayerTypeExtensions
{
    public static Color ToVisulaColor(this PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Dark => Color.black,
            PlayerType.Light => Color.white,
            _ => Color.black,
        };
    }

    public static Color32 ToColor32(this PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Dark => Color.red,
            PlayerType.Light => Color.green,
            _ => Color.black,
        };
    }

    public static Color ToColor(this PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Dark => Color.red,
            PlayerType.Light => Color.green,
            _ => Color.black,
        };
    }

    public static bool IsDeadly(this Color color, PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Dark => color.r >= 1.0f,
            PlayerType.Light => color.g >= 1.0f,
            _ => false,
        };
    }
}
