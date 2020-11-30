using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender {

	[InitializeOnLoad]
	public static class CustomToolbarRight {
		
		private static CustomToolbarSetting setting;
		
		private static GUIContent recompileBtn;
		private static GUIContent reserializeSelectedBtn;
		private static GUIContent reserializeAllBtn;
		private static int selectedFramerate = 60;

		static CustomToolbarRight() {
			setting = CustomToolbarSetting.GetOrCreateSetting();
			
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);

			recompileBtn = EditorGUIUtility.IconContent("WaitSpin05");
			recompileBtn.tooltip = "Recompile";

			reserializeSelectedBtn = EditorGUIUtility.IconContent("Refresh");
			reserializeSelectedBtn.tooltip = "Reserialize Selected Assets";

			reserializeAllBtn = EditorGUIUtility.IconContent("P4_Updating");
			reserializeAllBtn.tooltip = "Reserialize All Assets";
		}

		static void OnToolbarGUI() {
			int i = 0;
			for (; i < setting.elements.Count; ++i) 
				if(setting.elements[i] is ToolbarSides) 
					break;
			for (++i; i < setting.elements.Count; ++i)
				setting.elements[i].DrawInToolbar();

			DrawRecompileButton();
			DrawReserializeSelected();
			DrawReserializeAll();

			GUILayout.FlexibleSpace();
		}

		static void DrawRecompileButton() {
			if (GUILayout.Button(recompileBtn, ToolbarStyles.commandButtonStyle)) {
				UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
				Debug.Log("Recompile");
			}
		}

		static void DrawReserializeSelected() {
			if (GUILayout.Button(reserializeSelectedBtn, ToolbarStyles.commandButtonStyle)) {
				ForceReserializeAssetsUtils.ForceReserializeSelectedAssets();
			}
		}

		static void DrawReserializeAll() {
			if (GUILayout.Button(reserializeAllBtn, ToolbarStyles.commandButtonStyle)) {
				ForceReserializeAssetsUtils.ForceReserializeAllAssets();
			}
		}
	}
}