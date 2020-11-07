using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
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

		static List<SceneInfo> _scenes;
		static SceneInfo _sceneOpened;
		static int _selectedIndex;
		static string[] _displayedOptions;

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

			reloadSceneBtn = new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/LookDevResetEnv@2x.png", typeof(Texture2D)), "Reload scene");

			startFromFirstSceneBtn = new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/LookDevSingle1@2x.png", typeof(Texture2D)), "Start from 1 scene");

			LoadFromPlayerPrefs();
			EditorSceneManager.sceneOpened += HandleSceneOpened;
		}

		static void OnToolbarGUI() {
			GUILayout.FlexibleSpace();

			DrawSavingPrefsButton();
			DrawClearPrefsButton();
			DrawReloadSceneButton();
			DrawStartFromFirstSceneButton();

			GUILayout.FlexibleSpace();

			_selectedIndex = EditorGUILayout.Popup(_selectedIndex, _displayedOptions); ;

			GUI.enabled = _selectedIndex == 0;
			if (GUILayout.Button("+"))
				AddScene(_sceneOpened);

			GUI.enabled = _selectedIndex > 0;
			if (GUILayout.Button("-"))
				RemoveScene(_sceneOpened);

			GUI.enabled = true;
			if (GUI.changed && _selectedIndex > 0 && _scenes.Count > _selectedIndex - 1)
				EditorSceneManager.OpenScene(_scenes[_selectedIndex - 1].Path);
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

		static void RefreshDisplayedOptions() {
			_displayedOptions = new string[_scenes.Count + 1];
			_displayedOptions[0] = "Click on '+' to add current scene";

			for (int i = 0; i < _scenes.Count; i++)
				_displayedOptions[i + 1] = _scenes[i].Name;
		}

		static void HandleSceneOpened(Scene scene, OpenSceneMode mode) => SetOpenedScene(scene);

		static void SetOpenedScene(SceneInfo scene) {
			if (scene == null || string.IsNullOrEmpty(scene.Path))
				return;

			for (int i = 0; i < _scenes.Count; i++) {
				if (_scenes[i].Path == scene.Path) {
					_sceneOpened = _scenes[i];
					_selectedIndex = i + 1;
					SaveToPlayerPrefs(true);
					return;
				}
			}

			_sceneOpened = scene;
			_selectedIndex = 0;
			SaveToPlayerPrefs(true);
		}

		static void SetOpenedScene(Scene scene) => SetOpenedScene(new SceneInfo(scene));

		static void AddScene(SceneInfo scene) {
			if (scene == null)
				return;

			if (_scenes.Any(s => s.Path == scene.Path))
				RemoveScene(scene);

			_scenes.Add(scene);
			_selectedIndex = _scenes.Count;
			SetOpenedScene(scene);
			RefreshDisplayedOptions();
			SaveToPlayerPrefs();
		}

		static void RemoveScene(SceneInfo scene) {
			_scenes.Remove(scene);
			_selectedIndex = 0;
			RefreshDisplayedOptions();
			SaveToPlayerPrefs();
		}

		static void SaveToPlayerPrefs(bool onlyLatestOpenedScene = false) {
			if (!onlyLatestOpenedScene) {
				var serialized = string.Join(";", _scenes.Where(s => !string.IsNullOrEmpty(s.Path)).Select(s => s.Path));
				SetPref("SceneSelectionToolbar.Scenes", serialized);
			}

			if (_sceneOpened != null)
				SetPref("SceneSelectionToolbar.LatestOpenedScene", _sceneOpened.Path);
		}

		static void LoadFromPlayerPrefs() {
			var serialized = GetPref("SceneSelectionToolbar.Scenes");

			_scenes = serialized.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => new SceneInfo(s)).ToList();

			if (_scenes == null)
				_scenes = new List<SceneInfo>();

			serialized = GetPref("SceneSelectionToolbar.LatestOpenedScene");

			if (!string.IsNullOrEmpty(serialized))
				SetOpenedScene(new SceneInfo(serialized));

			RefreshDisplayedOptions();
		}

		static void SetPref(string name, string value) => EditorPrefs.SetString($"{Application.productName}_{name}", value);
		static string GetPref(string name) => EditorPrefs.GetString($"{Application.productName}_{name}");

		[Serializable]
		class SceneInfo {
			public SceneInfo() { }
			public SceneInfo(Scene scene) {
				Name = scene.name;
				Path = scene.path;
			}

			public SceneInfo(string path) {
				Name = System.IO.Path.GetFileNameWithoutExtension(path);
				Path = path;
			}

			public string Name;
			public string Path;
		}
	}
}
