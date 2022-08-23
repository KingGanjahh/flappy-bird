using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    [SerializeField] ScoreHandler scoreHandler;

    void Start()
    {
        Time.timeScale = 1;
        scoreHandler = FindObjectOfType<ScoreHandler>();
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;

        scoreHandler.CheckScore();

        if (scoreHandler.isHighScore)
        {
            scoreHandler.SubScore();
            scoreHandler.isHighScore = false;
        }
    }
    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
}
