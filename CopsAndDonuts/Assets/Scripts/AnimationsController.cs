using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationsController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private Animator animator;
    [SerializeField] private List<string> AnimationBools;

    private CharacterSelectionManager characterSelectionManager;
    private PlayerInputManager playerInputManager;
    private bool hasChecked;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        // CharacterSelectionManager is DontDestroyOnLoad so FindObjectOfType will always find it
        characterSelectionManager = FindObjectOfType<CharacterSelectionManager>();

        if (characterSelectionManager == null)
            Debug.Log("CharacterSelectionManager not found!");

        // PlayerInputManager is in the current scene so find it here
        playerInputManager = FindObjectOfType<PlayerInputManager>();

        if (playerInputManager == null)
            Debug.Log("PlayerInputManager not found!");
    }

    private void Update()
    {
        if (playerInputManager == null)
        {
            // Try finding it again in case scene just loaded
            playerInputManager = FindObjectOfType<PlayerInputManager>();
            return;
        }

        if (playerInputManager.playerCount == 3 && !hasChecked)
        {
            AssignSprite();
        }
    }

    private void AssignSprite()
    {
        hasChecked = true;

        if (characterSelectionManager == null)
        {
            Debug.Log("Cannot assign sprite — CharacterSelectionManager is null");
            return;
        }

        // Match by device so join order doesn't matter
        if (playerInput.devices.Count > 0)
        {
            InputDevice device = playerInput.devices[0];
            CharacterData selectedCharacter = GameSessionData.GetCharacterForDevice(device);

            if (selectedCharacter == null)
            {
                Debug.Log("No character found for device — player " + playerInput.playerIndex);
                return;
            }

            // Reset all animation bools first
            animator.SetBool("IsPlayer1", false);
            animator.SetBool("IsPlayer2", false);
            animator.SetBool("IsPlayer3", false);

            // Find which index this character is in the characters array
            CharacterData[] characters = characterSelectionManager.characters;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].characterName == selectedCharacter.characterName)
                {
                    // index 0 = IsPlayer3, index 1 = IsPlayer1, index 2 = IsPlayer2
                    // matching your original logic
                    if (i == 0) animator.SetBool("IsPlayer3", true);
                    else if (i == 1) animator.SetBool("IsPlayer1", true);
                    else if (i == 2) animator.SetBool("IsPlayer2", true);

                    Debug.Log("Player " + playerInput.playerIndex + " assigned: " + selectedCharacter.characterName + " (index " + i + ")");
                    break;
                }
            }

            // Apply portrait sprite
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null && selectedCharacter.portrait != null)
            {
                sr.sprite = selectedCharacter.portrait;
                Debug.Log("Sprite applied for: " + selectedCharacter.characterName);
            }
            else
            {
                Debug.Log("SpriteRenderer or portrait is null for player " + playerInput.playerIndex);
            }
        }
    }
}