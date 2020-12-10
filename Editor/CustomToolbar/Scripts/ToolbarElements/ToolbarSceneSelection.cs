using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;


[Serializable]
internal class ToolbarSceneSelection : BaseToolbarElement {
	public override string NameInList => "[Dropdown] Scene selection";
	
	[SerializeField] bool showSceneFolder = true;

	SceneData[] scenesPopupDisplay;
	string[] scenesPath;
	string[] scenesBuildPath;
	int selectedSceneIndex;

	public override void Init() {
		RefreshScenesList();
		EditorSceneManager.sceneOpened -= HandleSceneOpened;
		EditorSceneManager.sceneOpened += HandleSceneOpened;
	}

	protected override void OnDrawInList(Rect position) {
		position.width = 200.0f;
		showSceneFolder = EditorGUI.Toggle(position, "Group by folders", showSceneFolder);
	}

	protected override void OnDrawInToolbar() {
		EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
		DrawSceneDropdown();
		EditorGUI.EndDisabledGroup();
	}

	private void DrawSceneDropdown() {
		selectedSceneIndex = EditorGUILayout.Popup(selectedSceneIndex, scenesPopupDisplay.Select(e => e.popupDisplay).ToArray(), GUILayout.Width(WidthInToolbar));

		if (GUI.changed && 0 <= selectedSceneIndex && selectedSceneIndex < scenesPopupDisplay.Length) {
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
				foreach (var scenePath in scenesPath) {
					if ((scenePath) == scenesPopupDisplay[selectedSceneIndex].path) {
						EditorSceneManager.OpenScene(scenePath);
						break;
					}
				}
			}
		}

	}

	void RefreshScenesList() {
		List<SceneData> toDisplay = new List<SceneData>();

		selectedSceneIndex = -1;

		scenesBuildPath = EditorBuildSettings.scenes.Select(s => s.path).ToArray();

		string[] sceneGuids = AssetDatabase.FindAssets("t:scene", new string[] { "Assets" });
		scenesPath = new string[sceneGuids.Length];
		for (int i = 0; i < scenesPath.Length; ++i) {
			scenesPath[i] = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
		}

		Scene activeScene = SceneManager.GetActiveScene();
		int usedIds = scenesBuildPath.Length;

		for (int i = 0; i < scenesBuildPath.Length; ++i) {
			string name = GetSceneName(scenesBuildPath[i]);

			if (selectedSceneIndex == -1 && GetSceneName(name) == activeScene.name)
				selectedSceneIndex = i;

			GUIContent content = new GUIContent(name, EditorGUIUtility.Load("BuildSettings.Editor.Small") as Texture, "Open scene");

			toDisplay.Add(new SceneData() {
				path = scenesBuildPath[i],
				popupDisplay = content,
			});
		}

		toDisplay.Add(new SceneData() {
			path = "\0",
			popupDisplay = new GUIContent("\0"),
		});
		++usedIds;

		for (int i = 0; i < scenesPath.Length; ++i) {
			if (scenesBuildPath.Contains(scenesPath[i]))
				continue;
			string name;

			if (showSceneFolder) {
				string folderName = Path.GetFileName(Path.GetDirectoryName(scenesPath[i]));
				name = $"{folderName}/{GetSceneName(scenesPath[i])}";
			}
			else {
				name = GetSceneName(scenesPath[i]);
			}

			if (selectedSceneIndex == -1 && name == activeScene.name)
				selectedSceneIndex = usedIds;

			GUIContent content = new GUIContent(name, "Open scene");

			toDisplay.Add(new SceneData() {
				path = scenesPath[i],
				popupDisplay = content,
			});

			++usedIds;
		}

		scenesPopupDisplay = toDisplay.ToArray();
	}

	void HandleSceneOpened(Scene scene, OpenSceneMode mode) {
		RefreshScenesList();
	}

	string GetSceneName(string path) {
		path = path.Replace(".unity", "");
		return Path.GetFileName(path);
	}

	class SceneData {
		public string path;
		public GUIContent popupDisplay;
	}
}
