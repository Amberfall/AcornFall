using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimator : MonoBehaviour
{
    Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTreeGrowingAnimation()
    {
        animator.SetTrigger("GrowTree");
    }
}
