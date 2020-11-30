using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarSides : BaseToolbarElement {
	protected override void OnDrawInList(Rect position) {
		position.x += position.width + space;
		position.width = 200.0f;
		EditorGUI.LabelField(position, "Left side to up / Right side to bottom");
	}

	protected override void OnDrawInToolbar() {

	}
}
