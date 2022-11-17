using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainMenuPanel();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKey("escape"))
        //{
        //    exitGame();
        //}
    }
    void MainMenuPanel()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
