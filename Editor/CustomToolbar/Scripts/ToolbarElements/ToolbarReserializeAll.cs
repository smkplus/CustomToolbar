using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarReserializeAll : BaseToolbarElement {
	private static GUIContent reserializeAllBtn;

	public override string NameInList => "[Button] Reserialize all";

	public override void Init() {
		reserializeAllBtn = EditorGUIUtility.IconContent("P4_Updating");
		reserializeAllBtn.tooltip = "Reserialize All Assets";
	}

	protected override void OnDrawInList(Rect position) {

	}

	protected override void OnDrawInToolbar() {
		if (GUILayout.Button(reserializeAllBtn, UnityToolbarExtender.ToolbarStyles.commandButtonStyle)) {
			UnityToolbarExtender.ForceReserializeAssetsUtils.ForceReserializeAllAssets();
		}
	}
}
