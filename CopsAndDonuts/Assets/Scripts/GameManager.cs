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
    private PlayerInputManager playerInputManager;
    public GameObject pressPanel;

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // MUST be on so scores persist across levels
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (PlayerInput.all.Count >= 3)
        {
            if (playerInputManager != null)
            {
                playerInputManager.DisableJoining();
                Debug.Log("All 3 players joined - joining disabled");
            }
        }
    }

    private void Update()
    {
        if (playerInputManager != null && pressPanel != null)
        {
            if (playerInputManager.playerCount == 3)
            {
                pressPanel.SetActive(false);

            }
        }
    }


    public void PlateWon(int plateID)
    {
        if (plateID == 0) player1Score++;
        else if (plateID == 1) player2Score++;
        else if (plateID == 2) player3Score++;

        Debug.Log("Round " + currentRound + " of " + totalRounds + " complete");

        currentRound++;

        //if (currentRound > totalRounds)
        //{
        //    Debug.Log("All rounds done — loading FinalpANEL");
        //    ShowWinner();
        //}
    }

    public void ResetGame()
    {
        player1Score = 0;
        player2Score = 0;
        player3Score = 0;
        currentRound = 1;

        GameSessionData.SetSelectedCharacters(null, null, null);

        // Destroy all players
        PlayerInput[] players = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
        foreach (PlayerInput player in players)
        {
            Destroy(player.gameObject);
        }

        Debug.Log("Game reset — loading START");
        SceneManager.LoadScene("START");

        // Destroy GameManager last so it doesn't carry over
        Destroy(gameObject);
    }

}