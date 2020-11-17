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
		private static int selectedFramerate = 60;

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
			EditorGUILayout.LabelField("Time", GUILayout.Width(30));
			Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, 0f, 10f, GUILayout.Width(200));
			GUILayout.Space(10);

			EditorGUILayout.LabelField("FPS", GUILayout.Width(30));
			selectedFramerate = EditorGUILayout.IntSlider("", selectedFramerate, 1, 120, GUILayout.Width(200));
			if(EditorApplication.isPlaying && selectedFramerate != Application.targetFrameRate) {
				Application.targetFrameRate = selectedFramerate;
			}
			GUILayout.Space(10);

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