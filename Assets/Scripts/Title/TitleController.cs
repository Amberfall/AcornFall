using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.WebSocket;
using System;
using Random = UnityEngine.Random;

public class TitleController : MonoBehaviour
{
    public bool EditorCreateOurOwnHost;

    public TextMeshProUGUI LoadingTM;
    public TextMeshProUGUI ConnectionFailedTM;
    public TextMeshProUGUI WinCounterTM;
    public AudioSource MusicSource;
    public CanvasGroup ButtonsGroup;
    public CanvasGroup FullScreenImage;
    public CanvasGroup UIGroup;

    public GameObject Acorn;

    public AudioSource ButtonMouseOverSource;
    public AudioSource ButtonMouseClickSource;

    public TreeGrower TreePrefab;

    private void Awake()
    {
        ButtonsGroup.alpha = 0f;
        ButtonsGroup.interactable = false;
        
    }

    void Start()
    {
        Debug.Log("TitleStart");

        if (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost)
        {
            OnServerConnected();
            return;
        }
        else
        {
#if UNITY_EDITOR
            if (EditorCreateOurOwnHost)
            {
                // testing is in place, no need to connect to the cloud.
                // we can instead be our own server
                Debug.Log("Detected editor start. Creating our own local server and connecting to it");
                NetworkManager.Singleton.StartHost();
                OnServerConnected();
            }
            else
            {
                var transport = NetworkManager.Singleton.GetComponent<WebSocketTransport>();
                transport.ConnectAddress = "ggj.skipsabeatmusic.com";
                NetworkManager.Singleton.StartClient();

                StartCoroutine(CheckForConnection());
            }
#elif UNITY_SERVER
            // this is dumb, but it works, so it's not dumb.
            // except Unity is dumb.
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 10;   

            var transport = NetworkManager.Singleton.GetComponent<WebSocketTransport>();
            transport.ConnectAddress = "ggj.skipsabeatmusic.com";
            //transport.ConnectAddress = "localhost";
            Debug.Log($"Host address is: {transport.ConnectAddress}");
            NetworkManager.Singleton.StartServer();
#elif UNITY_WEBGL
            // auto connect to the server
            var transport = NetworkManager.Singleton.GetComponent<WebSocketTransport>();
            transport.ConnectAddress = "ggj.skipsabeatmusic.com";
            //transport.ConnectAddress = "34.105.51.253";
            var success = NetworkManager.Singleton.StartClient();
            if (success == false)
            {
                Debug.LogError("Could not connect to server");
            }
            StartCoroutine(CheckForConnection());
#endif
        }
    }

    public void OnServerConnected()
    {
        PlayIntroAnimation();
    }

    public IEnumerator CheckForConnection()
    {
        yield return new WaitForSeconds(5f);

        if (NetworkManager.Singleton.IsServer)
        {
            // all good
        }
        else if (NetworkManager.Singleton.IsConnectedClient == true)
        {
            // all good
        }
        else
        {
            // couldn't connect, continue on
            Debug.LogWarning("Couldn't connect to server, gonna do our own thing");
            NetworkManager.Singleton.Shutdown(false);

            ConnectionFailedTM.DOFade(1f, 0.5f);
            PlayIntroAnimation();
        }
    }

    public void PlayIntroAnimation()
    {
        LoadingTM.DOKill();
        LoadingTM.DOFade(0f, 0.5f);

        ButtonsGroup.DOFade(1f, 0.5f);
        ButtonsGroup.interactable = true;

        StartCoroutine(SpawnTrees());
    }

    IEnumerator SpawnTrees()
    {
        var sn = FindObjectOfType<ServerNetworking>();
        float minTime = 0.5f;
        int numTrees = Mathf.Min(sn.Wins1, 50);

        Random.InitState(0);
        for (int i = 0; i < numTrees; i++)
        {
            yield return new WaitForSeconds(minTime + Random.Range(minTime, minTime + 0.2f));

            minTime = Mathf.Max(0.1f, minTime - 0.05f);

            Instantiate(
                TreePrefab,
                new Vector3(Random.Range(-11, 11), 0, 0), 
                Quaternion.identity);
        }

        numTrees = Mathf.Min(sn.Wins2, 50);

        Random.InitState(1);
        for (int i = 0; i < numTrees; i++)
        {
            yield return new WaitForSeconds(minTime + Random.Range(minTime, minTime + 0.2f));

            minTime = Mathf.Max(0.1f, minTime - 0.05f);

            var newTree = Instantiate(
                TreePrefab,
                new Vector3(Random.Range(-11, 11), 0, 0),
                Quaternion.identity);
            newTree.MaxSize = 1;
        }

        numTrees = Mathf.Min(sn.Wins3, 50);

        Random.InitState(2);
        for (int i = 0; i < numTrees; i++)
        {
            yield return new WaitForSeconds(minTime + Random.Range(minTime, minTime + 0.2f));

            minTime = Mathf.Max(0.1f, minTime - 0.05f);

            var newTree = Instantiate(
                TreePrefab,
                new Vector3(Random.Range(-11, 11), 0, 0),
                Quaternion.identity);
            newTree.MaxSize = 1;
        }

        WinCounterTM.text = $"Players before you have grown {sn.Wins1 + sn.Wins2 + sn.Wins3} trees!";
        WinCounterTM.DOFade(1f, 0.25f);

    }

    public void PlayChangeToGameScene()
    {
        ButtonMouseClickSource.Play();

        var sequence = DOTween.Sequence();

        sequence.Append(Camera.main.transform.DOMove(new Vector3(6, -5, -10), 2f));
        sequence.Insert(0.1f, Acorn.transform.DOMoveY(-4, 1f).SetEase(Ease.InQuad));
        sequence.Insert(0.1f, MusicSource.DOFade(0f, 2f));
        sequence.Insert(0.1f, UIGroup.DOFade(0, 0.5f));
        sequence.Append(Camera.main.DOOrthoSize(1, 2f));
        sequence.Insert(2.5f, Camera.main.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360));
        sequence.Insert(3.0f, FullScreenImage.DOFade(1f, 1f));

        sequence.OnComplete(() =>
        {
            SceneManager.LoadScene("EasyLevel", LoadSceneMode.Single);
        });
    }

    public void ButtonMouseOver()
    {
        if (ButtonMouseOverSource.isPlaying == false)
            ButtonMouseOverSource.Play();
    }
}

