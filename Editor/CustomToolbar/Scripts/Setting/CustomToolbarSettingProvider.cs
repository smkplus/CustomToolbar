using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditorInternal;

namespace UnityToolbarExtender
{
	internal class CustomToolbarSettingProvider : SettingsProvider
	{
		private SerializedObject m_toolbarSetting;
		private CustomToolbarSetting setting;

		Vector2 scrollPos;
		ReorderableList elementsList;

		private class Styles
		{
			public static readonly GUIContent minFPS = new GUIContent("Minimum FPS");
			public static readonly GUIContent maxFPS = new GUIContent("Maximum FPS");
			public static readonly GUIContent limitFPS = new GUIContent("Limit FPS");
		}

		private const string SETTING_PATH = "Assets/Editor/Setting/CustomToolbarSetting.asset";
		
		public CustomToolbarSettingProvider(string path, SettingsScope scopes = SettingsScope.User) : base(
			path, scopes)
		{
			
		}

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			// base.OnActivate(searchContext, rootElement);
			m_toolbarSetting = CustomToolbarSetting.GetSerializedSetting();
			setting = (m_toolbarSetting.targetObject as CustomToolbarSetting);
		}

		public static bool IsSettingAvailable()
		{
			CustomToolbarSetting.GetOrCreateSetting();
			return File.Exists(SETTING_PATH);;
		}

		public override void OnGUI(string searchContext)
		{
			base.OnGUI(searchContext);

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			
			elementsList = elementsList ?? CustomToolbarReordableList.Create(setting.elements, OnMenuItemAdd);
			elementsList.DoLayoutList();

			EditorGUILayout.EndScrollView();

			m_toolbarSetting.ApplyModifiedProperties();
			if (GUI.changed) {
				EditorUtility.SetDirty(m_toolbarSetting.targetObject);
				ToolbarExtender.OnGUI();
			}
		}

		private void OnMenuItemAdd(object target) {
			setting.elements.Add(target as BaseToolbarElement);
			m_toolbarSetting.ApplyModifiedProperties();
		}

		[SettingsProvider]
		public static SettingsProvider CreateCustomToolbarSettingProvider()
		{
			if (IsSettingAvailable())
			{
				CustomToolbarSettingProvider provider = new CustomToolbarSettingProvider("Project/Custom Toolbar", SettingsScope.Project)
				{
					keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()
				};

				return provider;
			}

			return null;
		}
	}
}