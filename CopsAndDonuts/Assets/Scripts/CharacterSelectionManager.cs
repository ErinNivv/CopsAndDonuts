using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{

   
    [Header("Characters")]
    public CharacterData[] characters;

    [Header("Player Panels")]
    public PlayerSelectionPanel[] playerPanels;

    [Header("Start Game UI")]
    public GameObject startPromptObject;
    public TextMeshProUGUI startPromptText;
    public string firstLevelSceneName = "Level_01";

    [Header("Input Settings")]
    public float stickDeadzone = 0.5f;
    public float scrollCooldown = 0.3f;

    public int[] selectedCharacterIndex;
    private bool[] playerLocked;
    private float[] scrollTimers;
    private Gamepad[] gamepads;
   

    

    private void Awake()
    {
        selectedCharacterIndex = new int[3];
        playerLocked = new bool[3];
        scrollTimers = new float[3];

        if (startPromptObject != null)
            startPromptObject.SetActive(false);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CacheGamepads();

        for (int i = 0; i < 3; i++)
        {
            selectedCharacterIndex[i] = i < characters.Length ? i : 0;
            RefreshPanel(i);
        }
    }

    private void Update()
    {
        CacheGamepads();

        for (int p = 0; p < 3; p++)
        {
            if (playerLocked[p]) continue;

            Gamepad gp = GetGamepad(p);
            if (gp == null) continue;

            HandleScroll(p, gp);
            HandleConfirm(p, gp);
        }

        HandleStartGame();
    }

    private void HandleScroll(int playerIndex, Gamepad gp)
    {
        scrollTimers[playerIndex] -= Time.deltaTime;

        float horizontal = gp.leftStick.x.ReadValue();
        if (Mathf.Abs(horizontal) < stickDeadzone)
            horizontal = gp.dpad.x.ReadValue();

        if (Mathf.Abs(horizontal) > stickDeadzone && scrollTimers[playerIndex] <= 0f)
        {
            ChangeCharacter(playerIndex, horizontal > 0 ? 1 : -1);
            scrollTimers[playerIndex] = scrollCooldown;
        }

        if (Mathf.Abs(horizontal) < stickDeadzone)
            scrollTimers[playerIndex] = 0f;
    }

    private void HandleConfirm(int playerIndex, Gamepad gp)
    {
        if (!gp.buttonSouth.wasPressedThisFrame) return;

        int charIdx = selectedCharacterIndex[playerIndex];

        if (IsCharacterTakenByOther(charIdx, playerIndex))
        {
            playerPanels[playerIndex].PlayTakenFeedback();
            return;
        }

        playerLocked[playerIndex] = true;
        GameSessionData.SetDevice(playerIndex, gp); // save which controller this was
        playerPanels[playerIndex].ShowLocked(characters[charIdx]);
        CheckAllLocked();

        Debug.Log(charIdx);
    }

    private void CheckAllLocked()
    {
        bool allReady = playerLocked[0] && playerLocked[1] && playerLocked[2];
        if (startPromptObject != null)
            startPromptObject.SetActive(allReady);
    }

    private void HandleStartGame()
    {
        if (!(playerLocked[0] && playerLocked[1] && playerLocked[2])) return;

        foreach (Gamepad gp in Gamepad.all)
        {
            if (gp.buttonSouth.wasPressedThisFrame)
            {
                LoadFirstLevel();
                return;
            }
        }
    }

    private void LoadFirstLevel()
    {
        GameSessionData.SetSelectedCharacters(
            characters[selectedCharacterIndex[0]],
            characters[selectedCharacterIndex[1]],
            characters[selectedCharacterIndex[2]]
        );
        
        SceneManager.LoadScene("LEVEL 1");
    }

    private void ChangeCharacter(int playerIndex, int direction)
    {
        int newIdx = selectedCharacterIndex[playerIndex] + direction;
        if (newIdx < 0) newIdx = characters.Length - 1;
        if (newIdx >= characters.Length) newIdx = 0;

        selectedCharacterIndex[playerIndex] = newIdx;
        RefreshPanel(playerIndex);
    }

    private bool IsCharacterTakenByOther(int charIdx, int requestingPlayer)
    {
        for (int p = 0; p < 3; p++)
        {
            if (p == requestingPlayer) continue;
            if (playerLocked[p] && selectedCharacterIndex[p] == charIdx)
                return true;
        }
        return false;
    }

    private void RefreshPanel(int playerIndex)
    {
        if (playerPanels == null || playerIndex >= playerPanels.Length) return;
        int charIdx = selectedCharacterIndex[playerIndex];
        bool taken = IsCharacterTakenByOther(charIdx, playerIndex);
        playerPanels[playerIndex].UpdateDisplay(characters[charIdx], taken);
    }

    private void CacheGamepads()
    {
        gamepads = new Gamepad[3];
        var all = Gamepad.all;
        for (int i = 0; i < 3 && i < all.Count; i++)
            gamepads[i] = all[i];
    }

    private Gamepad GetGamepad(int playerIndex)
    {
        if (gamepads == null || playerIndex >= gamepads.Length) return null;
        return gamepads[playerIndex];
    }
}