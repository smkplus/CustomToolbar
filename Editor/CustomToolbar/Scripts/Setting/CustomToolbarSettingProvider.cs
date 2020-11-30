using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityToolbarExtender
{
	internal class CustomToolbarSettingProvider : SettingsProvider
	{
		private const int LEFT_VALUE = 1;
		private const int RIGHT_VALUE = 999;
		
		private SerializedObject m_toolbarSetting;

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

		public static bool IsSettingAvailable()
		{
			CustomToolbarSetting.GetOrCreateSetting();
			return File.Exists(SETTING_PATH);;
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			// base.OnActivate(searchContext, rootElement);
			m_toolbarSetting = CustomToolbarSetting.GetSerializedSetting();
		}

		public override void OnGUI(string searchContext)
		{
			base.OnGUI(searchContext);

			EditorGUILayout.IntSlider(m_toolbarSetting.FindProperty("minFPS"), LEFT_VALUE, RIGHT_VALUE, Styles.minFPS);
			EditorGUILayout.IntSlider(m_toolbarSetting.FindProperty("maxFPS"), LEFT_VALUE, RIGHT_VALUE, Styles.maxFPS);
			
			EditorGUILayout.PropertyField(m_toolbarSetting.FindProperty("limitFPS"), Styles.limitFPS);

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