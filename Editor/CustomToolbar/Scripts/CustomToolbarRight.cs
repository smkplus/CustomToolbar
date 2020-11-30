using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender {

	[InitializeOnLoad]
	public static class CustomToolbarRight {
		private static CustomToolbarSetting setting;

		static CustomToolbarRight() {
			setting = CustomToolbarSetting.GetOrCreateSetting();
			
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI() {
			int i = 0;
			for (; i < setting.elements.Count; ++i) 
				if(setting.elements[i] is ToolbarSides) 
					break;
			for (++i; i < setting.elements.Count; ++i)
				setting.elements[i].DrawInToolbar();

			GUILayout.FlexibleSpace();
		}
	}
}