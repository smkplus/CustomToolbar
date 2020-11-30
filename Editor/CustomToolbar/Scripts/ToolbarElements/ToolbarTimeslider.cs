using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarTimeslider : BaseToolbarElement {
	[SerializeField] float minTime = 1;
	[SerializeField] float maxTime = 120;

	public ToolbarTimeslider(float minTime = 0.0f, float maxTime = 10.0f) {
		this.minTime = minTime;
		this.maxTime = maxTime;
	}

	protected override void OnDrawInList(Rect position) {
		position.width = 200.0f;
		minTime = Mathf.RoundToInt(EditorGUI.FloatField(position, "Min Time", minTime));

		position.x += position.width + space;
		position.width = 200.0f;
		maxTime = Mathf.RoundToInt(EditorGUI.FloatField(position, "Max Time", maxTime));
	}

	protected override void OnDrawInToolbar() {
		EditorGUILayout.LabelField("Time", GUILayout.Width(30));
		Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, minTime, maxTime, GUILayout.Width(WidthInToolbar - 30.0f));
	}
}