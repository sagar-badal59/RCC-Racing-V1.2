using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void cancelBtnClicked()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }

    public void saveChangesBtnClicked()
    {

    }

    public void howtoPlayBtnClicked()
    {

    }

    public void NotificationBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Tournament);
    }

    public void featureBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Quest);
    }

    public void mainSoundBtnClicked()
    {

    }

    public void bgSoundBtnClicked()
    {

    }

    public void chatBtnClicked()
    {

    }

    public void infoBtnClicked()
    {

    }

}
