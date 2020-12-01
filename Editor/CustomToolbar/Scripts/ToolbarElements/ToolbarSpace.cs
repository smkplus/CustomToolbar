using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarSpace : BaseToolbarElement {
	public override string NameInList => $"[Empty space {WidthInToolbar} px]";

	public override void Init() {

	}

	public ToolbarSpace(float width = 10.0f) : base(width) {

	}

	protected override void OnDrawInList(Rect position) {
	
	}

	protected override void OnDrawInToolbar() {
		EditorGUILayout.Space(WidthInToolbar);
	}
}