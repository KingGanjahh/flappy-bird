using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using static LogInManager;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private string URL;

    [SerializeField] private string token, username;
    [SerializeField] public int score, highScore;

    [SerializeField] public TMP_Text scoreWindow;
    [SerializeField] public Text scoreText;

    [SerializeField] public bool isHighScore;

    void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("High Score", highScore);
        token = PlayerPrefs.GetString("token");
        username = PlayerPrefs.GetString("username");
    }

    public void CheckScore()
    {
        int currentScore = Int32.Parse(scoreText.text);
        if (currentScore > highScore)
        {
            isHighScore = true;

            highScore = score;
            PlayerPrefs.SetInt("High Score", highScore);
        }
    }

    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    public void SubScore()
    {
        token = PlayerPrefs.GetString("token");

        var data = new UserData();

        data.username = PlayerPrefs.GetString("username");
        data.score = PlayerPrefs.GetInt("High Score");

        var postData = JsonUtility.ToJson(data);

        StartCoroutine(SetScore(postData));
    }

    public IEnumerator SetScore(string postData)
    {
        var urlToWork = URL + "api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(urlToWork, postData);

        www.method = "PATCH";

        www.SetRequestHeader("content-type", "application/json");
        www.SetRequestHeader("x-token", token);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);

        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    public IEnumerator GetScores()
    {
        string url = URL + "api/usuarios" + "?limit=5&sort=true";
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.method = "GET";

        www.SetRequestHeader("content-type", "application/json");
        www.SetRequestHeader("x-token", token);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            Scores resData = JsonUtility.FromJson<Scores>(www.downloadHandler.text);

            foreach (var user in resData.usuarios)
            {
                scoreWindow.text += user.username.ToString() + "--" + user.score.ToString() + Environment.NewLine;
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

}

[Serializable]
public class ScoreData
{
    public string user_id;
    public int score;
    public int highScore;
}

[Serializable]
public class Scores
{
    public UserData[] usuarios;
}
