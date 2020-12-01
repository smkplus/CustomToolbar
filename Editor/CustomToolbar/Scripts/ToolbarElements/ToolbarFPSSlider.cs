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

	public override string NameInList => "[Slider] FPS";

	public override void Init() {
		selectedFramerate = 60;
	}

	public ToolbarFPSSlider(int minFPS = 1, int maxFPS = 120) : base(200) {
		this.minFPS = minFPS;
		this.maxFPS = maxFPS;
	}

	protected override void OnDrawInList(Rect position) {
		position.width = 70.0f;
		EditorGUI.LabelField(position, "Min FPS");

		position.x += position.width + FieldSizeSpace;
		position.width = 50.0f;
		minFPS = Mathf.RoundToInt(EditorGUI.IntField(position, "", minFPS));

		position.x += position.width + FieldSizeSpace;
		position.width = 70.0f;
		EditorGUI.LabelField(position, "Max FPS");

		position.x += position.width + FieldSizeSpace;
		position.width = 50.0f;
		maxFPS = Mathf.RoundToInt(EditorGUI.IntField(position, "", maxFPS));
	}

	protected override void OnDrawInToolbar() {
		EditorGUILayout.LabelField("FPS", GUILayout.Width(30));
		selectedFramerate = EditorGUILayout.IntSlider("", selectedFramerate, minFPS, maxFPS, GUILayout.Width(WidthInToolbar - 30.0f));
		if (EditorApplication.isPlaying && selectedFramerate != Application.targetFrameRate)
			Application.targetFrameRate = selectedFramerate;
	}
}
