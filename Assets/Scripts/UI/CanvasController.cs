using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waterRemainingText;
    [SerializeField] TextMeshProUGUI depthGaugeText;

    private int depthRequiredForWin;

    public void setDepthRequiredForWin(int depthRequired)
    {
        depthRequiredForWin = depthRequired;
    }
    public void setWaterRemainingText(object data)
    {
        var waterRemaining = (int)data;
        UnityEngine.Debug.Log("i've been called! water remaining is " + waterRemaining.ToString());
        waterRemainingText.text = waterRemaining.ToString();
    }
    public void setDepthGuageText(object data)
    {
        int currentDepth = (int)data;
        depthGaugeText.text = currentDepth.ToString() + " / " + depthRequiredForWin.ToString();
    }
}
