using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public int sceneGap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelBtnClicked(int _level)
    {
        int index = _level + 1;
        PlayerPrefs.SetString("SelectedSceneName", "Level_"+index);
        SceneManager.LoadScene("RCC Lobby (Photon PUN2", LoadSceneMode.Single);
        //Application.LoadLevel(_level+sceneGap);
    }
    public void playBtnClicked()
    {

    }
    public void exitBtnClicked()
    {

    }
}
