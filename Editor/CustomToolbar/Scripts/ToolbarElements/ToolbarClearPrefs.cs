using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

[Serializable]
internal class ToolbarClearPrefs : BaseToolbarElement {
	private static GUIContent clearPlayerPrefsBtn;

	public override string NameInList => "[Button] Clear prefs";

	public override void Init() {
		clearPlayerPrefsBtn = EditorGUIUtility.IconContent("SaveFromPlay");
		clearPlayerPrefsBtn.tooltip = "Clear player prefs";
	}

	protected override void OnDrawInList(Rect position) {

	}

	protected override void OnDrawInToolbar() {
		if (GUILayout.Button(clearPlayerPrefsBtn, ToolbarStyles.commandButtonStyle)) {
			PlayerPrefs.DeleteAll();
			Debug.Log("Clear Player Prefs");
		}
	}
}
