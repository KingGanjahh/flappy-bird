using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class HttpManager2 : MonoBehaviour
{
    [SerializeField] private string URL;
    [SerializeField] public TMP_Text scoreWindow;

    public void ClickSignUp()
    {
        var data = new AuthData();
        
        data.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;

        var postData = JsonUtility.ToJson(data);
        
        StartCoroutine(SignUp(postData));
    }

    IEnumerator SignUp(string postData)
    {
        var url = URL + "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network Error" +  www.error);
        }
        else if (www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            
            //StartCoroutine(LogIn(postData));
        }
        else
        {
            Debug.Log(www.error);
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

