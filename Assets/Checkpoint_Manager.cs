using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Checkpoint_Manager : Photon.Pun.MonoBehaviourPunCallbacks
{
    public Text lapCountText;
    public int totalRound;
    public int clearedRound;
    public GameWinPanel gameWinPanel;
    public GameLoosePanel gameLoosePanel;
    public List<PickupCheckpoint> checkpoints;
    public PickupCheckpoint lastCheckpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onNewCheckPointPicked(PickupCheckpoint newCheckpoint)
    {
        //if (lastCheckpoint != null)
        //{
        //    if (lastCheckpoint.index < newCheckpoint.index)
        //    {
        //        newCheckpoint.timesCollid++;
        //        if (newCheckpoint.index == checkpoints.Count) clearedRound++;
        //    }
        //}
        //else
        //{
        //    newCheckpoint.timesCollid++;
        //}
        //if(clearedRound==totalRound) gameWinPanel.showPanel();
        //lastCheckpoint = newCheckpoint;
        if(lastCheckpoint != null)
        {
            if (newCheckpoint.index == lastCheckpoint.index + 1)
            {
                newCheckpoint.timesCollid++;
                if (newCheckpoint.timesCollid > totalRound)
                {
                    //gameWinPanel.showPanel();
                    Debug.Log("Player id is : " + PhotonNetwork.LocalPlayer.UserId);
                    //Debug.Log("Nick Name : " + other.GetComponentInParent<PhotonView>().Owner.NickName);
                    //Constants.winnerplayerPostions.Add(index.ToString());
                    Constants.winnerplayerPostions.Add(PhotonNetwork.LocalPlayer.UserId);
                    //gameWinPanel.showPanel();
                    addwinuser();
                    
                }
                lastCheckpoint = newCheckpoint;
            }else if (lastCheckpoint.index == checkpoints.Count)
            {
                clearedRound++;
                newCheckpoint.timesCollid++;
                lapCountText.text = "Lap : "+clearedRound.ToString();
                if (newCheckpoint.timesCollid > totalRound){
                    Debug.Log("Win");
                    gameWinPanel.showPanel();
                }
                lastCheckpoint = newCheckpoint;
            }
        }
        else
        {
            newCheckpoint.timesCollid++;
            lastCheckpoint = newCheckpoint;
        }


        
    }

    public void addwinuser()
    {

        string[] winnerstring = new string[Constants.winnerplayerPostions.Count];
        winnerstring = System.Linq.Enumerable.ToArray(Constants.winnerplayerPostions);
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {"winnerlist",winnerstring}

            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("_________________________________________________________________________" + propertiesThatChanged.ToStringFull());
        object startTimeFromProps;
        if (propertiesThatChanged.ContainsKey("winnerlist") && propertiesThatChanged.TryGetValue("winnerlist", out startTimeFromProps))
        {
            //gameLoosePanel.showPanel();
            Constants.winnerplayerPostions.CopyTo((string[])startTimeFromProps);
            print(Constants.winnerplayerPostions.Count);
            if (Constants.winnerplayerPostions.Contains(PhotonNetwork.LocalPlayer.UserId)) gameWinPanel.showPanel();
            else gameLoosePanel.showPanel();
            foreach (var val in Constants.winnerplayerPostions)
                print(val);
        }
    }
}
