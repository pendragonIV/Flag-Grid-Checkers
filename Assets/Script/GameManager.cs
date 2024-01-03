using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    #region Game status
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGamePause = false;
    #endregion

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Win(int player)
    {
        isGameWin = true;

        gameScene.ShowWinPanel(player);
        Time.timeScale = 0;
    }

    public void ShowDrawPanel()
    {
        isGameLose = true;
        gameScene.ShowDrawPanel();
        Time.timeScale = 0;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    public void PauseGame()
    {
        isGamePause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePause = false;
        Time.timeScale = 1;
    }

    public bool IsGamePause()
    {
        return isGamePause;
    }
}

