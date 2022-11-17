//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple FPS Character controller used in demo scenes. Not professional.
/// You can use your 3rd party or any character controller instead of this script. 
/// </summary>
public class BCG_FPSController : MonoBehaviour {

	private bool canControl = true;

	#region Controller

	public float speed = 10.0f;     //	Maixmum directional speed.

	private float inputMovementY;       //	Input Y clamped (-1, 1f)
	private float inputMovementX;       //	Input X clamped (-1, 1f)

	#endregion

	#region Camera

	public Camera characterCamera;      //	Camera of the character.
	public float sensitivity = 5.0f;        //	Sensitivity of the camera.
	public float smoothing = 2.0f;      //	Smoothing factor of the camera.

	private Vector2 mouseInputVector;       //	Mouse input for the camera.

	// smooth the mouse moving
	private Vector2 smoothV;

	#endregion

	public BCG_Inputs inputs;

	void Start() {

		//	Find character camera at the start.
		if (!characterCamera)
			characterCamera = GetComponentInChildren<Camera>();

	}

	void FixedUpdate() {

		// Override canControl bool from BCG_EnterExitPlayer. 
		canControl = GetComponent<BCG_EnterExitPlayer>().canControl;

		//	If canControl is enabled, enable the camera, receive inputs from the player, and feed it.
		if (canControl)
			Controller();       //	Process the controller.

	}

	private void Update() {

		// Override canControl bool from BCG_EnterExitPlayer. 
		canControl = GetComponent<BCG_EnterExitPlayer>().canControl;

		//	If canControl is enabled, enable the camera, receive inputs from the player, and feed it.
		if (canControl) {

			characterCamera.gameObject.SetActive(true);
			Inputs();       //	Receive inputs form the player.
			Camera();       //	Process the camera.

		} else {

			characterCamera.gameObject.SetActive(false);

		}

	}

	/// <summary>
	/// Receive inputs from the player.
	/// </summary>
	private void Inputs() {

		inputs = BCG_InputManager.GetInputs();

		//	Receive keyboard inputs if controller type is not mobile. If controller type is mobile, inputs will be received by BCG_MobileCharacterController component attached to FPS/TPS Controller UI Canvas.
		if (!RCC_Settings.Instance.mobileControllerEnabled) {

			//	X and Y inputs based "Vertical" and "Horizontal" axes.
			inputMovementY = inputs.verticalInput * speed * Time.deltaTime;
			inputMovementX = inputs.horizonalInput * speed * Time.deltaTime;

			mouseInputVector += inputs.aim;
			mouseInputVector = new Vector2(mouseInputVector.x, Mathf.Clamp(mouseInputVector.y, -75f, 75f));

		} else {

			//	Receiving X and Y inputs from mobile inputs.
			inputMovementY = BCG_MobileCharacterController.move.y * speed * Time.deltaTime;
			inputMovementX = BCG_MobileCharacterController.move.x * speed * Time.deltaTime;

			// Mouse delta
			var mouseDelta = new Vector2(BCG_MobileCharacterController.mouse.x, BCG_MobileCharacterController.mouse.y);
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

			// the interpolated float result between the two float values
			smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
			smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);

			// incrementally add to the camera look
			mouseInputVector += smoothV;
			mouseInputVector = new Vector3(mouseInputVector.x, Mathf.Clamp(mouseInputVector.y, -75f, 75f));

		}

	}

	private void Controller() {

		// Translating the character with X and Y directions.
		transform.Translate(inputMovementX, 0, inputMovementY);

		//// Turn on the cursor
		//if (Input.GetKeyDown("escape"))
		//	Cursor.lockState = CursorLockMode.None;

	}

	private void Camera() {

		//	Setting rotations of the camera.
		characterCamera.transform.localRotation = Quaternion.AngleAxis(-mouseInputVector.y, Vector3.right);
		transform.localRotation = Quaternion.AngleAxis(mouseInputVector.x, transform.up);

	}

}