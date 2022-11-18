﻿//----------------------------------------------
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
using Photon;
using Photon.Pun;

/// <summary>
/// Streaming player input, or receiving data from server. And then feeds the RCC.
/// </summary>
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(RCC_CarControllerV3))]
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Network")]
public class RCC_PhotonNetwork : Photon.Pun.MonoBehaviourPunCallbacks, IPunObservable
{

	public bool connected = false;
	public bool isMine = false;
	public bool isRaceCompleted = false;

	// Main RCC, Rigidbody, and Main RCC Camera of the scene. 
	private RCC_CarControllerV3 carController;
	private RCC_WheelCollider[] wheelColliders;
	private Rigidbody rigid;

	// Vehicle position and rotation. Will send these to server.
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	// Used for projected (interpolated) position.
	private Vector3 currentVelocity;
	private float updateTime = 0;

	// All inputs for RCC. We will send these values if we own this vehicle. If this vehicle is owned by other player, receives all these inputs from server.
	private float gasInput = 0f;
	private float brakeInput = 0f;
	private float steerInput = 0f;
	private float handbrakeInput = 0f;
	private float boostInput = 0f;
	private float clutchInput = 0f;
	private int gear = 0;
	private int direction = 1;
	private bool changingGear = false;
	private bool semiAutomaticGear = false;
	private float fuelInput = 1f;
	private bool engineRunning = false;

	private float[] cambers;
	private RCC_CarControllerV3.WheelType _wheelTypeChoise;
	private bool canGoReverseNow;
	private float engineTorque;
	private float brakeTorque;
	private float minEngineRPM;
	private float maxEngineRPM;
	private float engineInertia;
	private bool useRevLimiter;
	private bool useExhaustFlame;
	private float highspeedsteerAngle;
	private float highspeedsteerAngleAtspeed;
	private float antiRollFrontHorizontal;
	private float antiRollRearHorizontal;
	private float antiRollVertical;
	private float maxspeed;
	private float engineHeat;
	private float engineHeatMultiplier;
	private int totalGears;
	private float gearShiftingDelay;
	private float gearShiftingThreshold;
	private float clutchInertia;
	private bool NGear;
	private float launched;
	private bool ABS;
	private bool TCS;
	private bool ESP;
	private bool steeringHelper;
	private bool tractionHelper;
	private bool applyCounterSteering;
	private bool useNOS;
	private bool useTurbo;

	// Lights.
	private bool lowBeamHeadLightsOn = false;
	private bool highBeamHeadLightsOn = false;

	// For Indicators.
	private RCC_CarControllerV3.IndicatorsOn indicatorsOn;

	// For Nickname Text
	private TextMesh nicknameText;

	void Start()
	{

		// Getting RCC, Rigidbody. 
		carController = GetComponent<RCC_CarControllerV3>();
		wheelColliders = GetComponentsInChildren<RCC_WheelCollider>();
		cambers = new float[wheelColliders.Length];
		rigid = GetComponent<Rigidbody>();

		GameObject nicknameTextGO = new GameObject("NickName TextMesh");
		nicknameTextGO.transform.SetParent(transform, false);
		nicknameTextGO.transform.localPosition = new Vector3(0f, 2f, 0f);
		nicknameTextGO.transform.localScale = new Vector3(.25f, .25f, .25f);
		nicknameText = nicknameTextGO.AddComponent<TextMesh>();
		nicknameText.anchor = TextAnchor.MiddleCenter;
		nicknameText.fontSize = 25;

		if (!gameObject.GetComponent<PhotonView>().ObservedComponents.Contains(this))
			gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);

		gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;

		GetValues();

		// Setting name of the gameobject with Photon View ID.
		gameObject.name = gameObject.name + photonView.ViewID;

	}

	void GetValues()
	{

		correctPlayerPos = transform.position;
		correctPlayerRot = transform.rotation;

		gasInput = carController.throttleInput;
		brakeInput = carController.brakeInput;
		steerInput = carController.steerInput;
		handbrakeInput = carController.handbrakeInput;
		boostInput = carController.boostInput;
		clutchInput = carController.clutchInput;
		gear = carController.currentGear;
		direction = carController.direction;
		changingGear = carController.changingGear;
		semiAutomaticGear = carController.semiAutomaticGear;

		fuelInput = carController.fuelInput;
		engineRunning = carController.engineRunning;
		lowBeamHeadLightsOn = carController.lowBeamHeadLightsOn;
		highBeamHeadLightsOn = carController.highBeamHeadLightsOn;
		indicatorsOn = carController.indicatorsOn;

		for (int i = 0; i < wheelColliders.Length; i++)
			cambers[i] = wheelColliders[i].camber;

		_wheelTypeChoise = carController.wheelTypeChoise;
		canGoReverseNow = carController.canGoReverseNow;
		engineTorque = carController.maxEngineTorque;
		brakeTorque = carController.brakeTorque;
		minEngineRPM = carController.minEngineRPM;
		maxEngineRPM = carController.maxEngineRPM;
		engineInertia = carController.engineInertia;
		useRevLimiter = carController.useRevLimiter;
		useExhaustFlame = carController.useExhaustFlame;
		highspeedsteerAngle = carController.highspeedsteerAngle;
		highspeedsteerAngleAtspeed = carController.highspeedsteerAngleAtspeed;
		antiRollFrontHorizontal = carController.antiRollFrontHorizontal;
		antiRollRearHorizontal = carController.antiRollRearHorizontal;
		antiRollVertical = carController.antiRollVertical;
		maxspeed = carController.maxspeed;
		engineHeat = carController.engineHeat;
		engineHeatMultiplier = carController.engineHeatRate;
		totalGears = carController.totalGears;
		gearShiftingDelay = carController.gearShiftingDelay;
		gearShiftingThreshold = carController.gearShiftingThreshold;
		clutchInertia = carController.clutchInertia;
		NGear = carController.NGear;
		launched = carController.launched;
		ABS = carController.ABS;
		TCS = carController.TCS;
		ESP = carController.ESP;
		steeringHelper = carController.steeringHelper;
		tractionHelper = carController.tractionHelper;
		applyCounterSteering = carController.useCounterSteering;
		useNOS = carController.useNOS;
		useTurbo = carController.useTurbo;

	}

	void FixedUpdate()
	{

		if (!PhotonNetwork.IsConnectedAndReady)
			return;

		if (!carController)
			return;

		isMine = photonView.IsMine;

		if (Constants.CompleteGameStartCountdownTimer && photonView.OwnershipTransfer == OwnershipOption.Fixed)
		{

			if (isRaceCompleted)
			{
				carController.canControl = false;
			}
			else
			{
				// If we are the owner of this vehicle, disable external controller and enable controller of the vehicle. Do opposite if we don't own this.
				carController.externalController = !isMine;
				carController.canControl = isMine;
				//print("----------------IsMine--------------" + isMine);
			}
		}

		// If we are not owner of this vehicle, receive all inputs from server.
		if (!isMine)
		{

			Vector3 projectedPosition = this.correctPlayerPos + currentVelocity * (Time.time - updateTime);

			if (Vector3.Distance(transform.position, correctPlayerPos) < 15f)
			{

				transform.position = Vector3.Lerp(transform.position, projectedPosition, Time.deltaTime * 5f);
				transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);

			}
			else
			{

				transform.position = correctPlayerPos;
				transform.rotation = correctPlayerRot;

			}

			carController.throttleInput = gasInput;
			carController.brakeInput = brakeInput;
			carController.steerInput = steerInput;
			carController.handbrakeInput = handbrakeInput;
			carController.boostInput = boostInput;
			carController.clutchInput = clutchInput;
			carController.currentGear = gear;
			carController.direction = direction;
			carController.changingGear = changingGear;
			carController.semiAutomaticGear = semiAutomaticGear;

			carController.fuelInput = fuelInput;
			carController.engineRunning = engineRunning;
			carController.lowBeamHeadLightsOn = lowBeamHeadLightsOn;
			carController.highBeamHeadLightsOn = highBeamHeadLightsOn;
			carController.indicatorsOn = indicatorsOn;

			for (int i = 0; i < wheelColliders.Length; i++)
				wheelColliders[i].camber = cambers[i];

			carController.wheelTypeChoise = _wheelTypeChoise;
			carController.canGoReverseNow = canGoReverseNow;
			carController.maxEngineTorque = engineTorque;
			carController.brakeTorque = brakeTorque;
			carController.minEngineRPM = minEngineRPM;
			carController.maxEngineRPM = maxEngineRPM;
			carController.engineInertia = engineInertia;
			carController.useRevLimiter = useRevLimiter;
			carController.useExhaustFlame = useExhaustFlame;
			carController.highspeedsteerAngle = highspeedsteerAngle;
			carController.highspeedsteerAngleAtspeed = highspeedsteerAngleAtspeed;
			carController.antiRollFrontHorizontal = antiRollFrontHorizontal;
			carController.antiRollRearHorizontal = antiRollRearHorizontal;
			carController.antiRollVertical = antiRollVertical;
			carController.maxspeed = maxspeed;
			carController.engineHeat = engineHeat;
			carController.engineHeatRate = engineHeatMultiplier;
			carController.totalGears = totalGears;
			carController.gearShiftingDelay = gearShiftingDelay;
			carController.gearShiftingThreshold = gearShiftingThreshold;
			carController.clutchInertia = clutchInertia;
			carController.NGear = NGear;
			carController.launched = launched;
			carController.ABS = ABS;
			carController.TCS = TCS;
			carController.ESP = ESP;
			carController.steeringHelper = steeringHelper;
			carController.tractionHelper = tractionHelper;
			carController.useCounterSteering = applyCounterSteering;
			carController.useNOS = useNOS;
			carController.useTurbo = useTurbo;

			if (nicknameText && photonView.Owner != null)
				nicknameText.text = photonView.Owner.NickName;
			else
				nicknameText.text = "";

			if (RCC_SceneManager.Instance.activeMainCamera)
			{

				nicknameText.transform.LookAt(RCC_SceneManager.Instance.activeMainCamera.transform);
				nicknameText.transform.rotation = Quaternion.Euler(nicknameText.transform.eulerAngles.x, nicknameText.transform.eulerAngles.y + 180f, nicknameText.transform.eulerAngles.z);

			}

		}
		else
		{

			if (nicknameText)
				nicknameText.text = "";

		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (!carController)
			return;

		// Sending all inputs, position, rotation, and velocity to the server.
		if (stream.IsWriting)
		{

			//We own this player: send the others our data
			stream.SendNext(carController.throttleInput);
			stream.SendNext(carController.brakeInput);
			stream.SendNext(carController.steerInput);
			stream.SendNext(carController.handbrakeInput);
			stream.SendNext(carController.boostInput);
			stream.SendNext(carController.clutchInput);
			stream.SendNext(carController.currentGear);
			stream.SendNext(carController.direction);
			stream.SendNext(carController.changingGear);
			stream.SendNext(carController.semiAutomaticGear);

			stream.SendNext(carController.fuelInput);
			stream.SendNext(carController.engineRunning);
			stream.SendNext(carController.lowBeamHeadLightsOn);
			stream.SendNext(carController.highBeamHeadLightsOn);
			stream.SendNext(carController.indicatorsOn);

			for (int i = 0; i < wheelColliders.Length; i++)
				stream.SendNext(wheelColliders[i].camber);

			stream.SendNext(carController.wheelTypeChoise);
			stream.SendNext(carController.canGoReverseNow);
			stream.SendNext(carController.maxEngineTorque);
			stream.SendNext(carController.brakeTorque);
			stream.SendNext(carController.minEngineRPM);
			stream.SendNext(carController.maxEngineRPM);
			stream.SendNext(carController.engineInertia);
			stream.SendNext(carController.useRevLimiter);
			stream.SendNext(carController.useExhaustFlame);
			stream.SendNext(carController.highspeedsteerAngle);
			stream.SendNext(carController.highspeedsteerAngleAtspeed);
			stream.SendNext(carController.antiRollFrontHorizontal);
			stream.SendNext(carController.antiRollRearHorizontal);
			stream.SendNext(carController.antiRollVertical);
			stream.SendNext(carController.maxspeed);
			stream.SendNext(carController.engineHeat);
			stream.SendNext(carController.engineHeatRate);
			stream.SendNext(carController.totalGears);
			stream.SendNext(carController.gearShiftingDelay);
			stream.SendNext(carController.gearShiftingThreshold);
			stream.SendNext(carController.clutchInertia);
			stream.SendNext(carController.NGear);
			stream.SendNext(carController.launched);
			stream.SendNext(carController.ABS);
			stream.SendNext(carController.TCS);
			stream.SendNext(carController.ESP);
			stream.SendNext(carController.steeringHelper);
			stream.SendNext(carController.tractionHelper);
			stream.SendNext(carController.useCounterSteering);
			stream.SendNext(carController.useNOS);
			stream.SendNext(carController.useTurbo);

			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(rigid.velocity);

		}
		else
		{

			// Network player, receiving all inputs, position, rotation, and velocity from server. 
			gasInput = (float)stream.ReceiveNext();
			brakeInput = (float)stream.ReceiveNext();
			steerInput = (float)stream.ReceiveNext();
			handbrakeInput = (float)stream.ReceiveNext();
			boostInput = (float)stream.ReceiveNext();
			clutchInput = (float)stream.ReceiveNext();
			gear = (int)stream.ReceiveNext();
			direction = (int)stream.ReceiveNext();
			changingGear = (bool)stream.ReceiveNext();
			semiAutomaticGear = (bool)stream.ReceiveNext();

			fuelInput = (float)stream.ReceiveNext();
			engineRunning = (bool)stream.ReceiveNext();
			lowBeamHeadLightsOn = (bool)stream.ReceiveNext();
			highBeamHeadLightsOn = (bool)stream.ReceiveNext();
			indicatorsOn = (RCC_CarControllerV3.IndicatorsOn)stream.ReceiveNext();

			for (int i = 0; i < wheelColliders.Length; i++)
				cambers[i] = (float)stream.ReceiveNext();

			_wheelTypeChoise = (RCC_CarControllerV3.WheelType)stream.ReceiveNext();
			canGoReverseNow = (bool)stream.ReceiveNext();
			engineTorque = (float)stream.ReceiveNext();
			brakeTorque = (float)stream.ReceiveNext();
			minEngineRPM = (float)stream.ReceiveNext();
			maxEngineRPM = (float)stream.ReceiveNext();
			engineInertia = (float)stream.ReceiveNext();
			useRevLimiter = (bool)stream.ReceiveNext();
			useExhaustFlame = (bool)stream.ReceiveNext();
			highspeedsteerAngle = (float)stream.ReceiveNext();
			highspeedsteerAngleAtspeed = (float)stream.ReceiveNext();
			antiRollFrontHorizontal = (float)stream.ReceiveNext();
			antiRollRearHorizontal = (float)stream.ReceiveNext();
			antiRollVertical = (float)stream.ReceiveNext();
			maxspeed = (float)stream.ReceiveNext();
			engineHeat = (float)stream.ReceiveNext();
			engineHeatMultiplier = (float)stream.ReceiveNext();
			totalGears = (int)stream.ReceiveNext();
			gearShiftingDelay = (float)stream.ReceiveNext();
			gearShiftingThreshold = (float)stream.ReceiveNext();
			clutchInertia = (float)stream.ReceiveNext();
			NGear = (bool)stream.ReceiveNext();
			launched = (float)stream.ReceiveNext();
			ABS = (bool)stream.ReceiveNext();
			TCS = (bool)stream.ReceiveNext();
			ESP = (bool)stream.ReceiveNext();
			steeringHelper = (bool)stream.ReceiveNext();
			tractionHelper = (bool)stream.ReceiveNext();
			applyCounterSteering = (bool)stream.ReceiveNext();
			useNOS = (bool)stream.ReceiveNext();
			useTurbo = (bool)stream.ReceiveNext();

			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
			currentVelocity = (Vector3)stream.ReceiveNext();

			updateTime = Time.time;

		}

	}

}
#endif