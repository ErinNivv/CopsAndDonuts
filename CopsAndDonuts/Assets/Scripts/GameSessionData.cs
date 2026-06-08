using UnityEngine;
using UnityEngine.InputSystem;

public static class GameSessionData
{
    public static CharacterData[] Players { get; private set; } = new CharacterData[3];
    public static InputDevice[] Devices { get; private set; } = new InputDevice[3];

    public static void SetSelectedCharacters(CharacterData p1, CharacterData p2, CharacterData p3)
    {
        Players[0] = p1;
        Players[1] = p2;
        Players[2] = p3;
    }

    public static void SetDevice(int playerIndex, InputDevice device)
    {
        Devices[playerIndex] = device;
    }

    public static CharacterData GetCharacterForDevice(InputDevice device)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Devices[i] == device)
                return Players[i];
        }
        return null;
    }

    public static void ResetData()
    {
        Players = new CharacterData[3];
        Devices = new InputDevice[3];
    }
}