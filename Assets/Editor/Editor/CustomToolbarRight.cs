using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender {

	[InitializeOnLoad]
	public static class CustomToolbarRight {
		private static GUIContent recompileBtn;
		private static GUIContent reserializeSelectedBtn;
		private static GUIContent reserializeAllBtn;

		static CustomToolbarRight() {
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);

			recompileBtn = EditorGUIUtility.IconContent("WaitSpin05");
			recompileBtn.tooltip = "Recompile";

			reserializeSelectedBtn = EditorGUIUtility.IconContent("Refresh");
			reserializeSelectedBtn.tooltip = "Reserialize Selected Assets";

			reserializeAllBtn = EditorGUIUtility.IconContent("P4_Updating");
			reserializeAllBtn.tooltip = "Reserialize All Assets";
		}

		static void OnToolbarGUI() {
			Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, 0f, 10f, GUILayout.Width(300));

			GUILayout.Space(20);


			DrawRecompileButton();
			DrawReserializeSelected();
			DrawReserializeAll();
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