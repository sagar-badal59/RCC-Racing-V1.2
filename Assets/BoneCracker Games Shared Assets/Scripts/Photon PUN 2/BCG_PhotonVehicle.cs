#if BCG_ENTEREXITPHOTON && PHOTON_UNITY_NETWORKING
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class BCG_PhotonVehicle : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks {

    private RCC_CarControllerV3 carController;

    void Awake() {

        carController = GetComponent<RCC_CarControllerV3>();
        PhotonNetwork.AddCallbackTarget(this);

    }

    void OnDestroy() {

        PhotonNetwork.RemoveCallbackTarget(this);

    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer) {

        if (targetView != base.photonView)
            return;

        base.photonView.TransferOwnership(requestingPlayer);

    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner) {
        
        if (targetView != base.photonView)
            return;

    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest) {
        
        throw new System.NotImplementedException();

    }

    void Update() {

        // If we are the owner of this vehicle, disable external controller and enable controller of the vehicle. Do opposite if we don't own this.
        if (photonView.AmOwner && photonView.IsMine) {

            carController.canControl = true;
            carController.externalController = false;

        } else {

            carController.canControl = false;
            carController.externalController = true;

        }

    }

}
#endif