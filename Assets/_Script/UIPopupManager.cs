using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupManager : SingletonMonoBehaviour<UIPopupManager>
{
    [SerializeField]
    GameObject[] popups;
    GameObject selectedPopup;
    public ePopup currentPopup;
    public ePopup prevPopup;
    [SerializeField]
    GameObject background;

    public enum ePopup
    {
        Setting,
        Loading,
        User_Data,
        Recover,
    }

    public void changePopup(ePopup next_mode)
    {
        prevPopup = currentPopup;
        currentPopup = next_mode;
        StartCoroutine(ChangePopupCoroutine(next_mode));
    }

    IEnumerator ChangePopupCoroutine(ePopup next_mode)
    {
        Debug.Log("New Mode is " + next_mode);
        switch (next_mode)
        {
            case ePopup.Setting:
                showSettingPopup();
                break;
            case ePopup.Loading:
                showLoadingPopup();
                break;
            case ePopup.User_Data:
                showUserData_Popup();
                break;
            case ePopup.Recover:
                showRecoveryPanel();
                break;
            
        }

        yield return null;
    }
    public void HideSelectedPopUp()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
    }

    void showSettingPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[0].SetActive(true);
        selectedPopup = popups[0];

    }
    void showLoadingPopup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[1].SetActive(true);
        selectedPopup = popups[1];
    }

    void showUserData_Popup()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        selectedPopup = popups[2];
        selectedPopup.SetActive(true);
    }

    void showRecoveryPanel()
    {
        if (selectedPopup != null) selectedPopup.gameObject.SetActive(false);
        //background.SetActive(true);
        popups[2].SetActive(true);
        selectedPopup = popups[2];
    }
}
