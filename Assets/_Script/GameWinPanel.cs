using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartBtnClicked()
    {
        hidePanel();
        Application.LoadLevel(Application.loadedLevel);
    }
    public void goToMainMenu()
    {
        hidePanel();
        Application.LoadLevel(0);
    }
    public void showPanel()
    {
        gameObject.SetActive(true);
    }
    public void hidePanel()
    {
        gameObject.SetActive(false);
    }
}
