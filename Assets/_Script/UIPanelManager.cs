using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : SingletonMonoBehaviour<UIPanelManager>
{
    [SerializeField]
    GameObject[] panels;
    GameObject selectedPanel;
    public ePanel currentPanel;
    public ePanel prevPanel;
    [SerializeField]
    GameObject background;

    public enum ePanel
    {
        Splash,
        MainMenu,
        Setting,
        Lobby,
        Party,
        Customize_Car,
        Customize_Avatar,
        Profile,
        Tournament,
        Shop,
        Quest,
        Level,
        Qualified

    }

    public void Start()
    {
        //Debug.Log("Time is ");
        //Debug.Log(UnixTime.ConvYYYYMMDD_HHMMSS(UnixTime.FromDateTime(DateTime.Now.Date)));
    }
    public void changeMode(ePanel next_mode)
    {
        prevPanel = currentPanel;
        currentPanel = next_mode;
        StartCoroutine(ChangeModeCoroutine(next_mode));
    }

    IEnumerator ChangeModeCoroutine(ePanel next_mode)
    {
        Debug.Log("New Mode is " + next_mode);
        switch (next_mode)
        {
            case ePanel.Splash:
                showSplashScreen();
                break;
            case ePanel.MainMenu:
                showMainMenuScreen();
                break;
            case ePanel.Setting:
                showSettingScreen();
                break;
            case ePanel.Lobby:
                showLobbyScreen();
                break;
            case ePanel.Party:
                showPartyScreen();
                break;
            case ePanel.Customize_Car:
                showCustomize_CarScreen();
                break;
            case ePanel.Customize_Avatar:
                showCustomize_AvatarScreen();
                break;
            case ePanel.Profile:
                showProfileScreen();
                break;
            case ePanel.Tournament:
                showTournamentScreen();
                break;
            case ePanel.Shop:
                showShopScreen();
                break;
            case ePanel.Quest:
                showQuestScreen();
                break;
            case ePanel.Level:
                showLevelScreen();
                break;
            case ePanel.Qualified:
                showQualifiedScreen();
                break;
        }

        yield return null;
    }

    void showSplashScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        //background.SetActive(true);
        panels[0].SetActive(true);
        selectedPanel = panels[0];

    }
    void showMainMenuScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        //background.SetActive(true);
        panels[1].SetActive(true);
        selectedPanel = panels[1];
    }

    void showSettingScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        //background.SetActive(true);
        panels[2].SetActive(true);
        selectedPanel = panels[2];
    }

    void showLobbyScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[3];
        selectedPanel.SetActive(true);
    }

    void showPartyScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[4];
        selectedPanel.SetActive(true);
    }

    void showCustomize_CarScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[5];
        selectedPanel.SetActive(true);
    }

    void showCustomize_AvatarScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[6];
        selectedPanel.SetActive(true);
    }

    void showProfileScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[7];
        selectedPanel.SetActive(true);
    }

    void showTournamentScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[8];
        selectedPanel.SetActive(true);
    }

    void showShopScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[9];
        selectedPanel.SetActive(true);
    }

    void showQuestScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[10];
        selectedPanel.SetActive(true);
    }
    void showLevelScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[11];
        selectedPanel.SetActive(true);
    }
    void showQualifiedScreen()
    {
        if (selectedPanel != null) selectedPanel.gameObject.SetActive(false);
        selectedPanel = panels[12];
        selectedPanel.SetActive(true);
    }
}
