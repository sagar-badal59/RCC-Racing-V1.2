using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class WalletManager : SingletonMonoBehaviour<WalletManager>
{
    [SerializeField]
    Text message;

    public JsonData jsonResponse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void connectToServer(string address)
    {
        Debug.Log("Calling Server");
        //NetworkConst.allRecordRes _allRecordRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postAddress data = new NetworkConst.postAddress();
        data.address = address;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.ConnectToWallet(json, (string data) =>
        {
            
            //_allRecordRes = JsonUtility.FromJson<NetworkConst.allRecordRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            
            //Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            //Debug.Log(jsonResponse);
            if ((bool)jsonResponse["success"])
            {
                //message.text = jsonResponse["data"].ToString();
                message.text = jsonResponse["message"].ToString();
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                //Debug.Log("Name " + _allRecordRes.data[0].name);
                //foreach (NetworkConst.record1 tmpObj in _allRecordRes.data)
                //{
                //    createNewObj(tmpObj);
                //}
                //viewPanel.gameObject.SetActive(true);
            }
            else
            {
                message.text = jsonResponse["message"].ToString();
                //message.text = jsonResponse["message"].ToString();
                //Debug.Log("Message is " + _allRecordRes.message);
            }
            Debug.Log(jsonResponse["message"].ToString());
            Debug.Log(json);

        });
    }
}
