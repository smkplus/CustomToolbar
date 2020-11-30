using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarFPSSlider : BaseToolbarElement {
	[SerializeField] int minFPS = 1;
	[SerializeField] int maxFPS = 120;

	int selectedFramerate;

	public ToolbarFPSSlider(int minFPS = 1, int maxFPS = 120) {
		this.minFPS = minFPS;
		this.maxFPS = maxFPS;
	}

	protected override void OnDrawInList(Rect position) {
		position.width = 200.0f;
		minFPS = Mathf.RoundToInt(EditorGUI.IntField(position, "Min FPS", minFPS));

		position.x += position.width + space;
		position.width = 200.0f;
		maxFPS = Mathf.RoundToInt(EditorGUI.IntField(position, "Max FPS", maxFPS));
	}

	protected override void OnDrawInToolbar() {
		EditorGUILayout.LabelField("FPS", GUILayout.Width(30));
		selectedFramerate = EditorGUILayout.IntSlider("", selectedFramerate, minFPS, maxFPS, GUILayout.Width(WidthInToolbar - 30.0f));
		if (EditorApplication.isPlaying && selectedFramerate != Application.targetFrameRate)
			Application.targetFrameRate = selectedFramerate;
	}
}
