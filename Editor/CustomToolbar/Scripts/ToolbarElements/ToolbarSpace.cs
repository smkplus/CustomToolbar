using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarSpace : BaseToolbarElement {
	public ToolbarSpace(float width = 10.0f) : base(width) {

	}

	protected override void OnDrawInList(Rect position) {
		position.x += position.width + FieldSizeSpace;
		position.width = 200.0f;
		EditorGUI.LabelField(position, "Empty space");
	}

	protected override void OnDrawInToolbar() {
		EditorGUILayout.Space(WidthInToolbar);
	}
}