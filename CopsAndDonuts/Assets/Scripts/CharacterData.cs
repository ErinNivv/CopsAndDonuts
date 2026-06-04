using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName = "Officer Name";
    [TextArea] public string description = "Character bio";
    public Sprite portrait;
    public Color panelColor = Color.white;
}