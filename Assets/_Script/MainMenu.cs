using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string key = "0xfc635bd2bc746d40095569e8f2328fa74b85687ce25d0032cd69366eb3d2a3756caecd4519728a7ce9ae327de83a848a620c0c1229362c33756c434232bb84ae1c";
        if (PlayerPrefs.GetString("Account") != "")
        {
            WalletManager.Instance.connectToServer(PlayerPrefs.GetString("Account"));
        }
        //else
        //{
        //    Debug.Log("Accound data not found");
        //    WalletManager.Instance.connectToServer(PlayerPrefs.GetString(key));
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Lobby);
    }
    public void settingBtnClicked()
    {
        UIPopupManager.Instance.changePopup(UIPopupManager.ePopup.Setting);
    }

    public void mainMenuBtnClicked()
    {
        UIPopupManager.Instance.changePopup(UIPopupManager.ePopup.User_Data);
    }
}
