using UnityEngine;

public static class GameSessionData
{
    public static CharacterData[] Players { get; private set; } = new CharacterData[3];

    public static void SetSelectedCharacters(CharacterData p1, CharacterData p2, CharacterData p3)
    {
        Players[0] = p1;
        Players[1] = p2;
        Players[2] = p3;
    }
}
