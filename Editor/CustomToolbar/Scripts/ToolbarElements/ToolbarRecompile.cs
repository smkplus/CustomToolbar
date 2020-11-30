using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarRecompile : BaseToolbarElement {
	private static GUIContent recompileBtn;

	public ToolbarRecompile() {
		recompileBtn = EditorGUIUtility.IconContent("WaitSpin05");
		recompileBtn.tooltip = "Recompile";
	}

	protected override void OnDrawInList(Rect position) {

	}

	protected override void OnDrawInToolbar() {
		if (GUILayout.Button(recompileBtn, UnityToolbarExtender.ToolbarStyles.commandButtonStyle)) {
			UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
			Debug.Log("Recompile");
		}
	}
}
