using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpManager : MonoBehaviour
{
    [SerializeField] private string URL;
    
    void Start()
    {
        
    }

    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        var url = URL + "/scores";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network Error" +  www.error);
        }
        else if (www.responseCode == 200)
        {
            ScoresData resData = JsonUtility.FromJson<ScoresData>(www.downloadHandler.text);

            foreach (var score in resData.scores)
            {
                Debug.Log(score.user_id + "|" + score.score);
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}

[System.Serializable]
public class ScoreValue
{
    public int user_id;
    public int score;
}

[System.Serializable]
public class ScoresData
{
    public ScoreValue[] scores;
}
