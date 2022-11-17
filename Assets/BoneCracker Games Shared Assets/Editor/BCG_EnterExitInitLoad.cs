//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------


using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class BCG_EnterExitInitLoad : MonoBehaviour {

	[InitializeOnLoad]
	public class InitOnLoad {

		static InitOnLoad(){

			BCG_SetScriptingSymbol.SetEnabled("BCG_ENTEREXIT", true);

            if (!EditorPrefs.HasKey("BCG" + BCG_EnterExitSettings.BCGVersion)){
				
				EditorPrefs.SetInt("BCG" + BCG_EnterExitSettings.BCGVersion, 1);
				Selection.activeObject = BCG_EnterExitSettings.Instance;

			}

		}

	}

}
