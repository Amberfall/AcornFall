using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource shrivelSound;

    public void PlayShrivelSound()
    {
        shrivelSound.Play();
    }
}