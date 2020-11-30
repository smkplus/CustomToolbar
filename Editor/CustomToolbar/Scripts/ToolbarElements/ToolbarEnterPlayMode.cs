using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
internal class ToolbarEnterPlayMode : BaseToolbarElement {
#if UNITY_2019_3_OR_NEWER
	static int selectedEnterPlayMode;
#endif

	protected override void OnDrawInList(Rect position) {

	}

	protected override void OnDrawInToolbar() {
#if UNITY_2019_3_OR_NEWER
		DrawEnterPlayModeOption();
#endif
	}

#if UNITY_2019_3_OR_NEWER
	readonly string[] enterPlayModeOption = new[]
	{
			"Disabled",
			"Reload All",
			"Reload Scene",
			"Reload Domain",
			"FastMode",
		};


	void DrawEnterPlayModeOption() {
		if (EditorSettings.enterPlayModeOptionsEnabled) {
			EnterPlayModeOptions option = EditorSettings.enterPlayModeOptions;
			selectedEnterPlayMode = (int)option + 1;
		}

		selectedEnterPlayMode = EditorGUILayout.Popup(selectedEnterPlayMode, enterPlayModeOption, GUILayout.Width(WidthInToolbar));

		if (GUI.changed && 0 <= selectedEnterPlayMode && selectedEnterPlayMode < enterPlayModeOption.Length) {
			EditorSettings.enterPlayModeOptionsEnabled = selectedEnterPlayMode != 0;
			EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions)(selectedEnterPlayMode - 1);
		}
	}
#endif
}
