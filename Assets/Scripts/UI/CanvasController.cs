using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waterRemainingText;
    [SerializeField] TextMeshProUGUI depthGaugeText;

    public void setWaterRemainingText(int waterRemaining)
    {
        waterRemainingText.text = waterRemaining.ToString();
    }
    public void setDepthGuageText(int currentDepth, int depthRequiredForWin)
    {
        depthGaugeText.text = currentDepth.ToString() + " / " + depthRequiredForWin.ToString();
    }
}
