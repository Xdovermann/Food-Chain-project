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


public class AccountManager : MonoBehaviour
{

    public string DatabaseConnection = "";

    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI connectionStatus;

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);

        WWW www = new WWW("https://localhost:44312/", form);

        yield return www;

        if(www.text[0] == '0')
        {
            DataBaseManager.username = this.username.text;
            //   DBManager.score =
        }
        else
        {
            Debug.Log("user login failed");
          
        }


    }

   

}
