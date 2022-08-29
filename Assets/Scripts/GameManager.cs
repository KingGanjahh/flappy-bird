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

        int actualScore = Score.score;

        ScoreData data = new ScoreData();

        if (actualScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            scoreHandler.SubScore(actualScore);
        }

        Time.timeScale = 0;
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
}
