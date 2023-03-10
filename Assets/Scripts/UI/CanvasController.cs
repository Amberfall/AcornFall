using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waterRemainingText;
    [SerializeField] TextMeshProUGUI depthGaugeText;

    [SerializeField] GameObject winPanel;
    public GameObject losePanel;
    [SerializeField] TextMeshProUGUI losePanelDepthText;

    private int depthRequiredForWin;

    [Header("Events")]
    public GameEvent pauseGame;
    public GameEvent resumeGame;
    public GameEvent restartLevel;
    public GameEvent playNextLevel;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space))
        {
            if(losePanel.activeSelf)
            {
                UnityEngine.Debug.Log("restarting game...");
                restartLevel.Raise(default);
            }
            else if(winPanel.activeSelf) 
            { 
                playNextLevel.Raise(default);
            }
        }
    }

    public void setDepthRequiredForWin(int depthRequired)
    {
        depthRequiredForWin = depthRequired;
    }
    public void setWaterRemainingText(object data)
    {
        var waterRemaining = (int)data;
        waterRemainingText.text = waterRemaining.ToString();
    }
    public void setDepthGuageText(object data)
    {
        int currentDepth = (int)data;
        depthGaugeText.text = currentDepth.ToString() + " / " + depthRequiredForWin.ToString() + " meters";
    }

    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
    }
    public void CloseWinPanel() 
    { 
        winPanel.SetActive(false);
    }
    public void OpenLosePanel()
    {
        losePanel.SetActive(true);
    }
    public void CloseLosePanel()
    {
        losePanel.SetActive(false);
    }

    public void setLosePanelDepthText(object data)
    {
        losePanelDepthText.text = "YOU DIED\n\n At a depth of: " + data.ToString() + " meters";
    }
}
