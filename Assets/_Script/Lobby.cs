using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void mainMenuBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void playBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Level);
    }
    public void carClicked()
    {

    }
}
