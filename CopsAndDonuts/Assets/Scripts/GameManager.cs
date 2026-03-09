using UnityEngine;
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