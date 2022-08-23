using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class LogInManager : MonoBehaviour
{
    [SerializeField] private string URL;

    private string token, username;

    private void Start()
    {
        token = PlayerPrefs.GetString("token");
        username = PlayerPrefs.GetString("username");

        StartCoroutine(GetProfile());
    }

    public void ClickSignUp()
    {
        var data = new AuthData();
        
        data.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<InputField>().text;

        var postData = JsonUtility.ToJson(data);
        
        StartCoroutine(SignUp(postData));
    }

    public void ClickLogIn()
    {
        var data = new AuthData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<InputField>().text;

        var postData = JsonUtility.ToJson(data);

        StartCoroutine(LogIn(postData));
    }

    IEnumerator SignUp(string postData)
    {
        var url = URL + "api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network Error" + www.error);
        }
        else if (www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);

            StartCoroutine(LogIn(postData));
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator LogIn(string postData)
    {
        var url = URL + "api/auth/login";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network Error" + www.error);
        }
        else if (www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);

            PlayerPrefs.SetString("token", resData.token);
            PlayerPrefs.SetString("username", resData.usuario.username);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator GetProfile()
    {
        string url = URL + "api/usuarios/" + username;
        UnityWebRequest www = UnityWebRequest.Get(url);


        www.SetRequestHeader("x-token", token);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            SceneManager.LoadScene(1);

        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }

    [Serializable]
    public class AuthData
    {
        public string username, password;
        public UserData usuario;
        public string token;
    }
    
    [Serializable]
    public class UserData
    {
        public string _id, username;
        public bool estado;
        public int score;
    }
}

