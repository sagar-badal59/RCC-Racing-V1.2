//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

#if RCC_PHOTON && PHOTON_UNITY_NETWORKING
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

/// <summary>
/// A simple manager script for photon demo scene. It has an array of networked spawnable player vehicles, public methods, restart, and quit application.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Demo Manager")]
public class RCC_PhotonDemo : Photon.Pun.MonoBehaviourPunCallbacks
{

	public bool reconnectIfFails = true;
	private bool connectedWithThis = false;

	internal int selectedCarIndex = 0;
	internal int selectedBehaviorIndex = 0;

	public Transform[] spawnPoints;
	public GameObject menu;

	private void Start()
	{

		if (reconnectIfFails && !PhotonNetwork.IsConnectedAndReady)
			ConnectToPhoton();
		else if (PhotonNetwork.IsConnectedAndReady)
		{
			//menu.SetActive(true);
			Debug.Log("Set Vechicle according to Actor Number  % Constants.NoOfPayers" + PhotonNetwork.LocalPlayer.ActorNumber+"%"+ Constants.NoOfPayers);
			SelectVehicle(PhotonNetwork.LocalPlayer.ActorNumber % Constants.NoOfPayers);
			Spawn();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();

		GameStartCountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
	}


	public override void OnDisable()
	{
		base.OnDisable();

		GameStartCountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
	}

	private void OnCountdownTimerIsExpired()
	{
		print("timer end");
		Constants.CompleteGameStartCountdownTimer = true;
	}

	public void Spawn()
	{

		int actorNo = PhotonNetwork.LocalPlayer.ActorNumber;

		if (actorNo > spawnPoints.Length)
		{

			while (actorNo > spawnPoints.Length)
				actorNo -= spawnPoints.Length;

		}

		Vector3 lastKnownPos = Vector3.zero;
		Quaternion lastKnownRot = Quaternion.identity;

		RCC_CarControllerV3 newVehicle;

		if (RCC_SceneManager.Instance.activePlayerVehicle)
		{

			lastKnownPos = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
			lastKnownRot = RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation;

		}

		if (lastKnownPos == Vector3.zero)
		{

			lastKnownPos = spawnPoints[actorNo - 1].position;
			lastKnownRot = spawnPoints[actorNo - 1].rotation;

		}

		lastKnownRot.x = 0f;
		lastKnownRot.z = 0f;

		if (RCC_SceneManager.Instance.activePlayerVehicle)
			PhotonNetwork.Destroy(RCC_SceneManager.Instance.activePlayerVehicle.gameObject);
		Debug.Log("Spawn New vechicle" + selectedCarIndex);
		newVehicle = PhotonNetwork.Instantiate("Photon Vehicles/" + RCC_PhotonDemoVehicles.Instance.vehicles[selectedCarIndex].name, lastKnownPos + (Vector3.up), lastKnownRot, 0).GetComponent<RCC_CarControllerV3>();

		Minimap.Instance.setVechicle(newVehicle.gameObject);
		RCC.RegisterPlayerVehicle(newVehicle);
		RCC.SetControl(newVehicle, false);
		//SetMobileController(3);
		if (RCC_SceneManager.Instance.activePlayerCamera)
			RCC_SceneManager.Instance.activePlayerCamera.SetTarget(newVehicle.gameObject);
		menu.SetActive(false);

	}

	/// <summary>
	/// Selects the vehicle.
	/// </summary>
	/// <param name="index">Index.</param>
	public void SelectVehicle(int index)
	{

		selectedCarIndex = index;

	}

	// An integer index value used for setting behavior mode.
	public void SetBehavior(int index)
	{

		selectedBehaviorIndex = index;

	}

	// Here we are setting new selected behavior to corresponding one.
	public void InitBehavior()
	{

		RCC.SetBehavior(selectedBehaviorIndex);

	}

	//	Sets the main controller type.
	public void SetController(int index)
	{

		RCC.SetController(index);

	}

	// Sets the mobile controller type.
	public void SetMobileController(int index)
	{

		switch (index)
		{

			case 0:
				RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);
				break;
			case 1:
				RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
				break;
			case 2:
				RCC.SetMobileController(RCC_Settings.MobileController.SteeringWheel);
				break;
			case 3:
				RCC.SetMobileController(RCC_Settings.MobileController.Joystick);
				break;

		}

	}

	/// <summary>
	/// Sets the quality.
	/// </summary>
	/// <param name="index">Index.</param>
	public void SetQuality(int index)
	{

		QualitySettings.SetQualityLevel(index);

	}

	// Simply restarting the current scene.
	public void RestartScene()
	{

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	// Simply quit application. Not working on Editor.
	public void Quit()
	{

		Application.Quit();

	}

	void ConnectToPhoton()
	{

		Debug.Log("Connecting to server");
		connectedWithThis = true;
		RCC_InfoLabel.Instance.ShowInfo("Entering to lobby");
		PhotonNetwork.NickName = "New Player " + Random.Range(0, 99999).ToString();
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{

		if (!connectedWithThis)
			return;

		Debug.Log("Connected to server");
		Debug.Log("Entering to lobby");
		RCC_InfoLabel.Instance.ShowInfo("Entering to lobby");
		PhotonNetwork.JoinLobby();

	}

	public override void OnJoinedLobby()
	{

		if (!connectedWithThis)
			return;

		Debug.Log("Entered to lobby");
		RCC_InfoLabel.Instance.ShowInfo("Creating / Joining Random Room");
		PhotonNetwork.JoinRandomRoom();

	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{

		if (!connectedWithThis)
			return;

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsOpen = true;
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = (byte)Constants.NoOfPayers;

		PhotonNetwork.JoinOrCreateRoom("New Room " + Random.Range(0, 999), roomOptions, TypedLobby.Default);

	}

	public override void OnJoinedRoom()
	{

		if (!connectedWithThis)
			return;

		//if (menu)
		//	menu.SetActive(true);
		Debug.Log("Set Vechicle according to Actor Number " + PhotonNetwork.LocalPlayer.ActorNumber);
		SelectVehicle(PhotonNetwork.LocalPlayer.ActorNumber);
		Spawn();
	}

	public override void OnCreatedRoom()
	{

		if (!connectedWithThis)
			return;

		//if (menu)
		//	menu.SetActive(true);		

	}

	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		//Debug.Log("_________________________________________________________________________"+propertiesThatChanged.ToStringFull());
		//object startTimeFromProps;
		//if (propertiesThatChanged.ContainsKey("winnerlist") && propertiesThatChanged.TryGetValue("winnerlist", out startTimeFromProps))
		//{
		//	Constants.winnerplayerPostions.CopyTo((string[])startTimeFromProps); 
		//	print(Constants.winnerplayerPostions.Count);
		//	foreach (var val in Constants.winnerplayerPostions)
		//		print(val);
		//}
	}
	

	public void addwinuser()
    {
		ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
			{
				{"winnerlist", (HashSet<string>) Constants.winnerplayerPostions}
			};
		PhotonNetwork.CurrentRoom.SetCustomProperties(props);
	}

}
#endif