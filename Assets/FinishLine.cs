using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviourPunCallbacks
{
    public GameObject WinnerPanel;
    public HashSet<string> playerPostions = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vehicle") || other.CompareTag("Player"))
        {
            playerPostions.Add(other.GetComponentInParent<PhotonView>().Owner.NickName);
            print(other.GetComponentInParent<PhotonView>().Owner.NickName+","+ playerPostions.Count);
            other.GetComponentInParent<RCC_PhotonNetwork>().isRaceCompleted = true;
            if((PhotonNetwork.CurrentRoom.Players.Count == 2 && AllRaceCompleted()) || playerPostions.Count > 2)
            {
                WinnerPanel.SetActive(true);
            }            
        }
    }

    public void NextGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) SceneManager.LoadScene(0);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.Players.Count == 1)
        {
           FindObjectOfType<RCC_PhotonNetwork>().isRaceCompleted = true;
           WinnerPanel.SetActive(true);
        }
    }

    public bool AllRaceCompleted()
    {
        foreach(var obj in FindObjectsOfType<RCC_PhotonNetwork>())
        {
            if (!obj.GetComponent<RCC_PhotonNetwork>().isRaceCompleted)
                return false;
        }
        return true;
    }
}
