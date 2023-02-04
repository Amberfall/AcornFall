using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionController : MonoBehaviour
{
    [SerializeField] private int depthRequiredToWin;

    private CanvasController canvasController;

    private void OnEnable()
    {
        canvasController = FindObjectOfType<CanvasController>();
    }

    private void Start()
    {
        canvasController.setDepthRequiredForWin(depthRequiredToWin);
    }
}
