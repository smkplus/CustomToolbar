using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender
{
    [InitializeOnLoad]
	public class CustomToolbarLeft {
		private static bool _deleteKeys = false;

		private static GUIContent savePassiveBtn;
		private static GUIContent saveActiveBtn;
		private static GUIContent clearPlayerPrefsBtn;
		private static GUIContent reloadSceneBtn;
		private static GUIContent startFromFirstSceneBtn;

		static CustomToolbarLeft() {
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
			EditorApplication.playModeStateChanged += LogPlayModeState;

			_deleteKeys = false;

			clearPlayerPrefsBtn = EditorGUIUtility.IconContent("SaveFromPlay");
			clearPlayerPrefsBtn.tooltip = "Clear player prefs";

			savePassiveBtn = EditorGUIUtility.IconContent("SavePassive");
			savePassiveBtn.tooltip = "Enable saving player prefs (currently NOT saving)";

			saveActiveBtn = EditorGUIUtility.IconContent("SaveActive");
			saveActiveBtn.tooltip = "Disable saving player prefs (currently saving)";

			reloadSceneBtn = new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Plugins/CustomToolbar/Editor/Icons/LookDevResetEnv@2x.png", typeof(Texture2D)), "Reload scene");

			startFromFirstSceneBtn = new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Plugins/CustomToolbar/Editor/Icons/LookDevSingle1@2x.png", typeof(Texture2D)), "Start from 1 scene");
		}

		static void OnToolbarGUI() {
			GUILayout.FlexibleSpace();

			DrawSavingPrefsButton();
			DrawClearPrefsButton();
			DrawReloadSceneButton();
			DrawStartFromFirstSceneButton();
		}

		private static void LogPlayModeState(PlayModeStateChange state) {
			if (state == PlayModeStateChange.EnteredEditMode && EditorPrefs.HasKey("LastActiveSceneToolbar")) {
				EditorSceneManager.OpenScene(
					SceneUtility.GetScenePathByBuildIndex(EditorPrefs.GetInt("LastActiveSceneToolbar")));
				EditorPrefs.DeleteKey("LastActiveSceneToolbar");
			}
		}

		private static void DrawSavingPrefsButton() {
			if (_deleteKeys) {
				if (GUILayout.Button(savePassiveBtn, ToolbarStyles.commandButtonStyle)) {
					_deleteKeys = false;
					Debug.Log("Enable saving player prefs");
				}
			}
			else {
				if (GUILayout.Button(saveActiveBtn, ToolbarStyles.commandButtonStyle)) {
					_deleteKeys = true;
					Debug.Log("Disable saving player prefs");
				}
			}

			if (_deleteKeys) {
				PlayerPrefs.DeleteAll();
			}
		}

		private static void DrawClearPrefsButton() {
			if (GUILayout.Button(clearPlayerPrefsBtn, ToolbarStyles.commandButtonStyle)) {
				PlayerPrefs.DeleteAll();
				Debug.Log("Clear Player Prefs");
			}
		}

		private static void DrawReloadSceneButton() {
			EditorGUIUtility.SetIconSize(new Vector2(17, 17));
			if (GUILayout.Button(reloadSceneBtn, ToolbarStyles.commandButtonStyle)) {
				if (EditorApplication.isPlaying) {
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}

		private static void DrawStartFromFirstSceneButton() {
			if (GUILayout.Button(startFromFirstSceneBtn, ToolbarStyles.commandButtonStyle)) {
				if (!EditorApplication.isPlaying) {
					EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
					EditorPrefs.SetInt("LastActiveSceneToolbar", EditorSceneManager.GetActiveScene().buildIndex);
					EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
				}

				EditorApplication.isPlaying = !EditorApplication.isPlaying;
			}
		}
	}
}