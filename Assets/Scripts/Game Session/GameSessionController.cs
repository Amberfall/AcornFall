using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionController : MonoBehaviour
{
    [SerializeField] private int depthRequiredToWin;

    private CanvasController canvasController;

    private Scene currentLevel;

    private void OnEnable()
    {
        canvasController = FindObjectOfType<CanvasController>();
    }

    private void Start()
    {
        canvasController.setDepthRequiredForWin(depthRequiredToWin);
        currentLevel = SceneManager.GetActiveScene();
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
        ResumeGame();
        SceneManager.LoadScene(currentLevel.name);
    }
    public void LoseGame()
    {
        //TODO: animate withering root, wait a moment, then do the rest of this logic
        PauseGame();
        canvasController.OpenLosePanel();
    }

}
