using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waterRemainingText;
    [SerializeField] TextMeshProUGUI depthGaugeText;

    public void setWaterRemainingText(object data)
    {
        var waterRemaining = (int)data;
        UnityEngine.Debug.Log("i've been called! water remaining is " + waterRemaining.ToString());
        waterRemainingText.text = waterRemaining.ToString();
    }
    public void setDepthGuageText(int currentDepth, int depthRequiredForWin)
    {
        depthGaugeText.text = currentDepth.ToString() + " / " + depthRequiredForWin.ToString();
    }
}
