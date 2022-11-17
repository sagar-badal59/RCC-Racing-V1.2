// NetworkingManager.cs
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class NetworkingManager : SingletonMonoBehaviour<NetworkingManager>
{


    public static string BASE_URL = "https://espsofttech.org:6040/api/";
    [SerializeField]
    GameObject blackoutImage;
    public Image image;
    public AudioSource audioSource;

    UnityWebRequest www;

    public delegate void onSuccess(string Text);
    public delegate void onError(string Text);


    public const string MatchEmailPattern =
      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public bool validateEmail(string email)
    {
        Debug.Log("Email is " + email);
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    public bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    public string Encorde64(string src)
    {
        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(src);
        string encodedText = Convert.ToBase64String(bytesToEncode);
        return encodedText;
    }
    public string Decode64(string src64)
    {
        src64 = src64.Replace("\"", "");
        byte[] decodedBytes = Convert.FromBase64String(src64);
        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
        return decodedText;
    }
    IEnumerator GETData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            Debug.Log("Erro: " + www.error);
            HideBlackout();
        }
        else
        {
            HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + www.responseCode);
            Debug.Log("Download handler data" + www.downloadHandler.data);
            successCallBack(www.downloadHandler.text);
        }
    }

    public IEnumerator POSTData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        Debug.Log("Data is ");
        Debug.Log(data);

        var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.uploadHandler.contentType = "application/x-www-form-urlencoded";
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            HideBlackout();
            Debug.Log("Erro: " + request.error);
            Debug.Log("<color=red>Error: " + request.error + "</color>");
        }
        else
        {
            //HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log("Response is : " + request.downloadHandler.text);
            Debug.Log("<color=yellow>Success: "+request.downloadHandler.text+ "</color>");
            successCallBack(request.downloadHandler.text);
        }

    }

    public IEnumerator POSTData1(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        Debug.Log("<color=blue>URL : " + url + "</color>");
        ShowBlackout();
        Debug.Log("Data is ");
        Debug.Log(data);

        www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        www.SetRequestHeader("Host", "<calculated when request is sent>");
        www.SetRequestHeader("User-Agent", "application/json");
        www.SetRequestHeader("Accept", "*/*");
        www.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
        www.SetRequestHeader("Connection", "keep-alive");


        //www.SetRequestHeader("Content-Type", "application/json");
        //if(!string.IsNullOrEmpty(CreatorData.instance.Token))
        //{
        //    Debug.Log("Token     "+CreatorData.instance.Token);
        //    www.SetRequestHeader("Authorization", CreatorData.instance.Token);
        //}

        //request.SetRequestHeader("password", "Admin123#");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            HideBlackout();
            Debug.Log("Erro: " + www.error);
            Debug.Log("<color=red>Error: " + www.error + "</color>");
        }
        else
        {
            //HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + www.responseCode);
            Debug.Log("Response is : " + www.downloadHandler.text);
            Debug.Log("<color=yellow>Success: " + www.downloadHandler.text + "</color>");
            successCallBack(www.downloadHandler.text);
        }
        HideBlackout();

    }

    public IEnumerator PATCHData(string url, string data, onSuccess successCallBack, onError errorCallBack = null)
    {
        ShowBlackout();
        Debug.Log("Data is " + data);

        var request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            HideBlackout();
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            HideBlackout();
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            successCallBack(request.downloadHandler.text);
        }

    }
    public void ShowBlackout()
    {
        blackoutImage.SetActive(true);
    }
    public void HideBlackout()
    {
        blackoutImage.SetActive(false);
    }




    public void APIPostLoginData(string json, onSuccess successCallback)
    {
        string url = BASE_URL+"login";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void APIPostRegisterData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "registration";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void APIPostRecoverData(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "forgot-password";

        StartCoroutine(POSTData(url, json, successCallback));
    }

    public void ConnectToWallet(string json, onSuccess successCallback)
    {
        string url = BASE_URL + "getNftOfOwnerByAddress";

        StartCoroutine(POSTData1(url, json, successCallback));
    }

}