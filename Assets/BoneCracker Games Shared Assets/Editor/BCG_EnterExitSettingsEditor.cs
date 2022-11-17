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
using UnityEditor;
using System;

[CustomEditor(typeof(BCG_EnterExitSettings))]
public class BCG_EnterExitSettingsEditor : Editor {

	public bool EnablePhotonEnterExit{
		
		get{
			
			bool _bool = BCG_EnterExitSettings.Instance.enableEnterExit_Photon;
			return _bool;

		}

		set{
			
			bool _bool = BCG_EnterExitSettings.Instance.enableEnterExit_Photon;

			if(_bool == value)
				return;

			BCG_EnterExitSettings.Instance.enableEnterExit_Photon = value;
            BCG_SetScriptingSymbol.SetEnabled("BCG_ENTEREXITPHOTON", value);

        }

	}

	public override void OnInspectorGUI () {

		serializedObject.Update();

		

		EditorGUILayout.PropertyField(serializedObject.FindProperty("enterExitVehicleKB"), new GUIContent("Enter Exit Vehicle"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("keepEnginesAlive"), new GUIContent("Keep Engines Alive"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("enterExitSpeedLimit"), new GUIContent("Enter Exit Speed Limit"));

		EnablePhotonEnterExit = EditorGUILayout.ToggleLeft(new GUIContent("Enable Photon Enter Exit", "It will enable Photon Enter Exit support for all BCG vehicles."), EnablePhotonEnterExit);

		if (EnablePhotonEnterExit) {
		
		
		
		}

		EditorGUILayout.LabelField("BCG Enter Exit  " + BCG_EnterExitSettings.BCGVersion + " \nBoneCracker Games", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		EditorGUILayout.LabelField("Created by Buğra Özdoğanlar", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		serializedObject.ApplyModifiedProperties();

	}

}
