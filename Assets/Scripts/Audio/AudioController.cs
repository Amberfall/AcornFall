using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource shrivelSound;
    [SerializeField] AudioSource treeGrowingSound;

    public void PlayShrivelSound()
    {
        shrivelSound.Play();
    }

    public void PlayTreeGrowingSound()
    {
        treeGrowingSound.Play();
    }
}
