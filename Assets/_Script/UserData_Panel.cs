using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UserData_Panel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.DOMove(new Vector3(10, 0, 0), 2).From();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void orderBtnClicked()
    {
        closePopup();
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Shop);
    }
    public void profileBtnClicked()
    {
        closePopup();
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Profile);
    }

    public void win_lossBtnClicked()
    {
        closePopup();
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Shop);
    }

    public void contactBtnClicked()
    {
        //need to open an url in browser
    }

    public void help_fnqBtnClicked()
    {
        //need to open an url in browser
    }

    public void closePopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }

}
