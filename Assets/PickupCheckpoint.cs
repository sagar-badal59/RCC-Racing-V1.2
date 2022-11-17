using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PickupCheckpoint : MonoBehaviour
{

    public int index;
    public int timesCollid;
    public LayerMask layerMask;
    public GameWinPanel gameWinPanel;
    public GameLoosePanel gameLoosePanel;
    public Checkpoint_Manager checkPointManager;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            checkPointManager.onNewCheckPointPicked(this);
            //timesCollid++;
            //if (timesCollid >= 3)
            //{
            //    Debug.Log("Win the game");
            //    gameWinPanel.showPanel();
            //}
            //RCC_PhotonNetwork.
            //Debug.Log("Player id is : "+PhotonNetwork.LocalPlayer.UserId);
            //Debug.Log("Nick Name : "+other.GetComponentInParent<PhotonView>().Owner.NickName );
            ////Constants.winnerplayerPostions.Add(index.ToString());
            //Constants.winnerplayerPostions.Add(PhotonNetwork.LocalPlayer.UserId);
            ////gameWinPanel.showPanel();
            //addwinuser();
        }
        
        
    }
    
}
