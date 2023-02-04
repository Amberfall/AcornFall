using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;
using TMPro;

public class TitleController : MonoBehaviour
{
    public TextMeshProUGUI LoadingTM;
    public CanvasGroup ButtonsGroup;
    public CanvasGroup FullScreenImage;
    public CanvasGroup UIGroup;

    ServerNetworkData _snd;


    private void Awake()
    {

        ButtonsGroup.alpha = 0f;
        ButtonsGroup.interactable = false;

        _snd = FindObjectOfType<ServerNetworkData>();
        if (_snd.HasRead)
        {
            Debug.Log("wow, data was already read!");
            NetworkDataRead();
        }
        else
        {
        }
    }

    public void NetworkDataRead()
    {
        LoadingTM.DOKill();
        LoadingTM.DOFade(0f, 0.5f);

        ButtonsGroup.DOFade(1f, 0.5f);
        ButtonsGroup.interactable = true;
    }

    public void ChangeToGameScene()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(Camera.main.transform.DOMove(new Vector3(5, -5, -10), 2f));
        sequence.Insert(0.1f, UIGroup.DOFade(0, 0.5f));
        sequence.Append(Camera.main.DOOrthoSize(1, 2f));
        sequence.Insert(2.5f, Camera.main.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360));
        sequence.Insert(3.0f, FullScreenImage.DOFade(1f, 1f));

        sequence.OnComplete(() =>
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        });
    }
}
