using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int player1Score;
    public int player2Score;
    public int player3Score;

    public int currentRound = 1;
    public int totalRounds = 5;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = playerInput.playerIndex;

        CharacterData selectedCharacter = GameSessionData.Players[index];

        if (selectedCharacter == null) return;

        SpriteRenderer sr = playerInput.GetComponent<SpriteRenderer>();
        if (sr != null && selectedCharacter.portrait != null)
            sr.sprite = selectedCharacter.portrait;

        Debug.Log("Player " + index + " joined as " + selectedCharacter.characterName);
    }

    public void PlayerWon(int playerIndex)
    {
        if (playerIndex == 0)
            player1Score++;

        else if (playerIndex == 1)
            player2Score++;

        else if (playerIndex == 2)
            player3Score++;

        currentRound++;

        if (currentRound > totalRounds)
        {
            SceneManager.LoadScene("FinalScene");
        }
    }
}