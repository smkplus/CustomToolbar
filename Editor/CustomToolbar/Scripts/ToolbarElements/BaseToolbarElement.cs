using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbarExtender;
using UnityEditor;

[Serializable]
abstract internal class BaseToolbarElement {
	[SerializeField] protected bool IsEnabled = true;
	[SerializeField] protected float WidthInToolbar;

	protected const float space = 10.0f;
	protected const float singleCharWidth = 7.0f;
	protected const float width = 50.0f;

	public BaseToolbarElement() : this(100.0f) {

	}

	public BaseToolbarElement(float widthInToolbar) {
		WidthInToolbar = widthInToolbar;
	}

	public void DrawInList(Rect position) {
		position.y += 2;
		position.height -= 4;

		position.x += space;
		position.width = 15.0f;
		IsEnabled = EditorGUI.Toggle(position, IsEnabled);

		position.x += position.width + space;
		position.width = singleCharWidth * 4;
		EditorGUI.LabelField(position, "Size");

		position.x += position.width + space;
		position.width = width;
		WidthInToolbar = EditorGUI.IntField(position, (int)WidthInToolbar);

		position.x += position.width + space;

		EditorGUI.BeginDisabledGroup(!IsEnabled);
		OnDrawInList(position);
		EditorGUI.EndDisabledGroup();
	}
	public void DrawInToolbar() {
		if(IsEnabled)
			OnDrawInToolbar();
	}


	abstract protected void OnDrawInList(Rect position);
	abstract protected void OnDrawInToolbar();
}
