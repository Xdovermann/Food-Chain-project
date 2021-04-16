using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;


public class AccountManager : MonoBehaviour
{

    public string DatabaseConnection = "";

    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI connectionStatus;

    public void CallLogin()
    {
       // Application.OpenURL("https://localhost:44386/LeaderBoardEntries/LoginUnity.cs");
        StartCoroutine(Upload());
    }



    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post("https://localhost:44386/LeaderBoardEntries/LoginUnity", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("connnectionError");
            }
            if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log("dataprocessingerror");
            }
            if (www.result == UnityWebRequest.Result.InProgress)
            {
                Debug.Log("inprogress");
            }
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("protocolerror");
            }
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Succes");
            }
            
        }
    }

    //IEnumerator Login()
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("name", username.text);
    //    form.AddField("password", password.text);

    //    WWW www = new WWW("https://localhost:44386/LeaderBoardEntries/LoginUnity/", form);

    //    yield return www;

    //    if (www.text == "")
    //    {
    //        // DataBaseManager.username = this.username.text;
    //        //   DBManager.score =
    //        Debug.Log("return empty");
    //    }
    //    else
    //    {
    //        Debug.Log("user login failed" + www);

    //    }

    //    // string LoginURL  = "https://localhost:44386/LeaderBoardEntries/LoginUnity/";

    //    //  UnityWebRequest LoginInfoRequest = UnityWebRequest.Get(LoginURL);
    //}



}
