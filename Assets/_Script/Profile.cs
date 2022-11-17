using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
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
    public void ridesHistoryBtnClicked()
    {
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void tncBtnClicked()
    {
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void settingBtnClicked()
    {
        UIPopupManager.Instance.changePopup(UIPopupManager.ePopup.Setting);
    }
    public void supportCenterBtnClicked()
    {
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void editProfileBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Customize_Avatar);
    }
    public void carClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Customize_Car);
    }
}
