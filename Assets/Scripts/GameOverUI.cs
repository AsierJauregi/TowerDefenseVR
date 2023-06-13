using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private Canvas gameOverCanvas;
    private bool dataReported = false;
    
    private void Awake()
    {
        gameOverCanvas = GetComponent<Canvas>();
        gameOverCanvas.enabled = false;
    }

    public void EnableGameOverScreen()
    {
        gameOverCanvas.enabled = true;
        if (!dataReported)
        {
            GameLogic.GameInstance.ReportGameData();
            dataReported = true;
        }
    }
    public void Restart()
    {
        Time.timeScale = 1;
        GameLogic.GameInstance.LoadGame();
    }
}
