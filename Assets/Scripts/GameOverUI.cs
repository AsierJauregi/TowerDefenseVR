using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Canvas cameraCanvas;
    
    private void Awake()
    {
        cameraCanvas.enabled = false;

    }

    public void Restart()
    {
        Time.timeScale = 1;
        GameLogic.GameInstance.LoadGame();
    }
}
