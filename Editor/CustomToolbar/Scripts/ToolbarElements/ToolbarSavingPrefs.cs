using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

[Serializable]
internal class ToolbarSavingPrefs : BaseToolbarElement {
	private static GUIContent savePassiveBtn;
	private static GUIContent saveActiveBtn;
	private static bool _deleteKeys = false;

	public override string NameInList => "[Button]Disable saving prefs";

	public override void Init() {
		_deleteKeys = false;

		savePassiveBtn = EditorGUIUtility.IconContent("SavePassive");
		savePassiveBtn.tooltip = "Enable saving player prefs (currently NOT saving)";

		saveActiveBtn = EditorGUIUtility.IconContent("SaveActive");
		saveActiveBtn.tooltip = "Disable saving player prefs (currently saving)";
	}

	protected override void OnDrawInList(Rect position) {

	}

	protected override void OnDrawInToolbar() {
		if (_deleteKeys) {
			if (GUILayout.Button(savePassiveBtn, ToolbarStyles.commandButtonStyle)) {
				_deleteKeys = false;
				Debug.Log("Enable saving player prefs");
			}
		}
		else {
			if (GUILayout.Button(saveActiveBtn, ToolbarStyles.commandButtonStyle)) {
				_deleteKeys = true;
				Debug.Log("Disable saving player prefs");
			}
		}

		if (_deleteKeys) {
			PlayerPrefs.DeleteAll();
		}
	}
}
