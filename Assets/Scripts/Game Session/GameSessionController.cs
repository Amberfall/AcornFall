using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionController : MonoBehaviour
{
    [SerializeField] private int depthRequiredToWin;


    [Header("Events")]
    public GameEvent playerWon;
    public GameEvent receivedNetworkingData;

    private CanvasController canvasController;
    private AudioController audioController;

    private Scene currentLevel;
    private ServerNetworking serverNetworking;

    private void OnEnable()
    {
        canvasController = FindObjectOfType<CanvasController>();
        audioController = FindObjectOfType<AudioController>();
        serverNetworking = FindObjectOfType<ServerNetworking>();
    }

    private void Start()
    {
        canvasController.setDepthRequiredForWin(depthRequiredToWin);
        currentLevel = SceneManager.GetActiveScene();
        SendCoordListForWaterDrops();
        //SendCoordListForWaterDropsTest();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    public void RestartLevel()
    {
        UnityEngine.Debug.Log("restarting from game controller...");
        ResumeGame();
        SceneManager.LoadScene(currentLevel.name);
    }
    public void PlayNextLevel()
    {
        if(currentLevel.buildIndex < 3)
        {
            SceneManager.LoadScene(currentLevel.buildIndex + 1);
        }
        else
        {
            RestartLevel();
        }
        
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void LoseGame()
    {
        //TODO: animate withering root, wait a moment, then do the rest of this logic
        
        StartCoroutine(DeathSequence());
    }

    public void CheckForWin(object data)
    {
        if((int)data >= depthRequiredToWin)
        {
            UnityEngine.Debug.Log("reached required depth for win condition.");
            playerWon.Raise(default);
        }
    }
    public void WinGame()
    {
        if(serverNetworking!= null)
        {
            serverNetworking.RecordWin(currentLevel.buildIndex);
        }
        
    }

    IEnumerator DeathSequence()
    {
        PauseGame();
        yield return new WaitForSecondsRealtime(.25f);
        PlayDeathShrivelSound();
        yield return new WaitForSecondsRealtime(.3f);
        canvasController.OpenLosePanel();
        yield break;
    }

    private void PlayDeathShrivelSound()
    {
        if(audioController != null) 
        {
            audioController.PlayShrivelSound();
        }
    }

    public void RecordLoseCoord(Vector3Int coordinate)
    {
        UnityEngine.Debug.Log("attempting to record loss... Checking for network.");

        if (serverNetworking != null)
        {
            Vector3Int lossCoordinate = new Vector3Int();
            lossCoordinate.x = coordinate.x; 
            lossCoordinate.y = coordinate.y;
            int sceneNumber = currentLevel.buildIndex;
            lossCoordinate.z = sceneNumber;
            serverNetworking.RecordLoss(lossCoordinate);

            UnityEngine.Debug.Log("recorded a loss at " + lossCoordinate);
        }
    }

    private void SendCoordListForWaterDrops()
    {
        UnityEngine.Debug.Log("checking for networking script...");
        if(serverNetworking != null) 
        {
            UnityEngine.Debug.Log("found network. preparing list of water tiles.");
            List<Vector3Int> coordList = new List<Vector3Int>();

            coordList = serverNetworking.Bonuses;

            foreach( Vector3Int bonus in coordList )
            {
                UnityEngine.Debug.Log("found previous player coord at " + bonus);
            }

            List<Vector3Int> thisLevelCoordList = new List<Vector3Int>();

            foreach(Vector3Int coord in coordList) 
            { 
                if(coord.z == currentLevel.buildIndex)
                {
                    thisLevelCoordList.Add(coord);
                }
            }

            if(thisLevelCoordList.Count > 0)
            {
                UnityEngine.Debug.Log("there are previous drops to spawn. Working on it...");
                receivedNetworkingData.Raise(thisLevelCoordList);
            }
            
        }
        

    }
    
    //SendCoordListForWaterDrops Test
    private void SendCoordListForWaterDropsTest()
    {
        List<Vector3Int> testCoordList = new List<Vector3Int>();

        for(int i = 0; i < 10; i++)
        {
            Vector3Int testCoord = new Vector3Int(-i,-i,-i);
            testCoordList.Add(testCoord);
        }
        receivedNetworkingData.Raise(testCoordList);
    }

}
