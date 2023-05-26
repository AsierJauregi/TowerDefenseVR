using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Canvas cameraCanvas;
    private GameObject gameOverPanel;
    
    private void Awake()
    {
        cameraCanvas.gameObject.SetActive(false);
        cameraCanvas.GetComponent<Image>().enabled = false;
        gameOverPanel = this.gameObject;
        gameOverPanel.SetActive(false);
    }

    public void EnableGameOverScreen()
    {
        cameraCanvas.gameObject.SetActive(true);
        cameraCanvas.GetComponent<Image>().enabled = true;
        gameOverPanel.SetActive(true);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        GameLogic.GameInstance.LoadGame();
    }
}
