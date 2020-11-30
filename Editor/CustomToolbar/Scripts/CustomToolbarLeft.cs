using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender
{
    [InitializeOnLoad]
	public class CustomToolbarLeft {
		private static CustomToolbarSetting setting;

		static CustomToolbarLeft() {
			setting = CustomToolbarSetting.GetOrCreateSetting();
			
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI() {
			GUILayout.FlexibleSpace();

			for (int i = 0; i < setting.elements.Count; ++i) {
				if (setting.elements[i] is ToolbarSides)
					break;
				setting.elements[i].DrawInToolbar();
			}
		}
	}
}
