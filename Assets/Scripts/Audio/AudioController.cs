using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource shrivelSound;
    [SerializeField] AudioSource treeGrowingSound;
    [SerializeField] AudioSource bgm;
    [SerializeField] float BGMmaxVolume = 1.0f;
    [SerializeField] float BGMminVolume = 0.25f;

    public void PlayShrivelSound()
    {
        shrivelSound.Play();
    }

    public void PlayTreeGrowingSound()
    {
        treeGrowingSound.Play();
    }
    public void lowerBGM()
    {
        StartCoroutine(GraduallyLowerBGMVolume());
    }
    IEnumerator GraduallyLowerBGMVolume()
    {
        do
        {
            bgm.volume = Mathf.MoveTowards(bgm.volume, BGMminVolume, Time.unscaledTime);
            yield return new WaitForEndOfFrame();
        }
        while (bgm.volume > BGMminVolume);
        yield break;
    }
}
