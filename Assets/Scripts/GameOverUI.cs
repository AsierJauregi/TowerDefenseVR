using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private Canvas gameOverCanvas;
    
    private void Awake()
    {
        gameOverCanvas = GetComponent<Canvas>();
        gameOverCanvas.enabled = false;
    }

    public void EnableGameOverScreen()
    {
        gameOverCanvas.enabled = true;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        GameLogic.GameInstance.LoadGame();
    }
}
