using UnityEngine;

public enum PlayerType
{
    Black = 0,
    White = 1
}

public static class PlayerTypeExtensions
{
    public static Color32 ToColor32(this PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Black => Color.red,
            PlayerType.White => Color.green,
            _ => Color.black,
        };
    }

    public static Color ToColor(this PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Black => Color.red,
            PlayerType.White => Color.green,
            _ => Color.black,
        };
    }

    public static bool IsDeadly(this Color color, PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Black => color.r >= 1.0f,
            PlayerType.White => color.g >= 1.0f,
            _ => false,
        };
    }
}
