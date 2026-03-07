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

        if(playerInput.playerIndex == 0 )
        {
            animator.SetBool("IsPlayer1", true);
        }
        if(playerInput.playerIndex == 1 )
        {
            animator.SetBool("IsPlayer2", true);
        }
        else if(playerInput.playerIndex == 2 )
        {
            animator.SetBool("IsPlayer3", true);
        }
    }


    //public void PlaySideSlide()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[0], true);
    //}

    //public void PlaySideRun()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[1], true);
    //}

    //public void PlayBackRun()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[2], true);
    //}

    //public void PlayBackSlide()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[3], true);
    //}

    //public void PlayFrontRun()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[4], true);
    //}

    //public void PlayFrontSlide()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[5], true);
    //}

    //public void PlayIdle()
    //{
    //    for (int i = 0; i < AnimationBools.Count; i++)
    //    {
    //        animator.SetBool(AnimationBools[i], false);
    //    }
    //    animator.SetBool(AnimationBools[6], true);
    //}

}
