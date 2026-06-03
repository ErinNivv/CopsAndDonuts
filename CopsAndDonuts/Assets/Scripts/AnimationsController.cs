using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationsController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private Animator animator;
    [SerializeField] private List<string> AnimationBools;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        int index = playerInput.playerIndex;
        CharacterData selectedCharacter = GameSessionData.Players[index];

        if (selectedCharacter == null)
        {
            Debug.Log("No character data found for player " + index);
            return;
        }

      
        if (selectedCharacter.characterName == "Reddy")
        {
            animator.SetBool("IsPlayer1", true);
        }
        else if (selectedCharacter.characterName == "Greeny")
        {
            animator.SetBool("IsPlayer2", true);
        }
        else if (selectedCharacter.characterName == "Bluey")
        {
            animator.SetBool("IsPlayer3", true);
        }

        Debug.Log("Player " + index + " assigned character: " + selectedCharacter.characterName);
    }
}
