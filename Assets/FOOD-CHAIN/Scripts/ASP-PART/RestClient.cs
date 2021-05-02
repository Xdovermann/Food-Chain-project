using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RestClient : MonoBehaviour
{
    public static RestClient restClient;

    public string webURL = "";
    // Start is called before the first frame update
    void Awake()
    {
        restClient = this;
    }


    public void GetButton()
    {
        StartCoroutine(Get(webURL, GetPlayers));
    }

    //public void DeleteButton()
    //{
    //    StartCoroutine(Delete(webURL))
    //} 
  public IEnumerator Get(string url,System.Action<PlayerProfileList> playerListcallBack)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("connection error");
        }else if(www.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError("dataprocessing error");
        }
        else if(www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("protocol error");
        }else if(www.result == UnityWebRequest.Result.Success)
        {
            if (www.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                
               PlayerProfileList myObject = JsonUtility.FromJson<PlayerProfileList>("{\"Players\":" + jsonResult.ToString() + "}");

                Debug.Log(myObject.Players.Count);
                //  string holder = "{\"players\":" + jsonResult + "}";

                // Debug.Log(holder);
                //   PlayerProfileList players = JsonUtility.FromJson<PlayerProfileList>(holder);
                //  Debug.Log(players.Players.Count);

                //   PlayerProfileList players =  JsonUtility.FromJson<PlayerProfileList>(jsonResult);

                 playerListcallBack(myObject);

            }
        }


    }

    public IEnumerator Delete(string url,int id)
    {
        string urlWithParams = string.Format("{0}{1}", url, id);
        UnityWebRequest www = UnityWebRequest.Delete(urlWithParams);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("connection error");
        }
        else if (www.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError("dataprocessing error");
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("protocol error");
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            if (www.isDone)
            {
                Debug.Log(string.Format("deleted player with Id: {0}", id));

            }
        }


    }

    public IEnumerator Post(string url, PlayerProfile newPlayer)
    {
        string jsonData = JsonUtility.ToJson(newPlayer);
        UnityWebRequest www = UnityWebRequest.Post(url, jsonData);

        www.uploadHandler.contentType = "application/json";
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData)) ;

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("connection error");
        }
        else if (www.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError("dataprocessing error");
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("protocol error");
        }
        else if (www.result == UnityWebRequest.Result.Success)
        {
            if (www.isDone)
            {
                

            }
        }


    }

    void GetPlayers(PlayerProfileList playerlist)
    {
        foreach (PlayerProfile player in playerlist.Players)
        {
            Debug.Log("Plater ID :" + player.id);
            Debug.Log("Plater Username :" + player.username);
            Debug.Log("Plater score :" + player.score);
        }
    }
}
