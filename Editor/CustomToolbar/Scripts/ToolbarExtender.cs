using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender
{
	[InitializeOnLoad]
	public static class ToolbarExtender
	{
		static int m_toolCount;
		static GUIStyle m_commandStyle = null;

		private static CustomToolbarSetting setting;

		static ToolbarExtender()
		{
			EditorApplication.update += Init;
			EditorApplication.playModeStateChanged += OnChangePlayMode;
		}

		static void OnChangePlayMode(PlayModeStateChange state) {
			if(state == PlayModeStateChange.EnteredPlayMode)
				InitElements();
		}	

		static void Init() {
			EditorApplication.update -= Init;
			InitElements();
		}

		public static void InitElements() {
		Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

#if UNITY_2019_1_OR_NEWER
			string fieldName = "k_ToolCount";
#else
			string fieldName = "s_ShownToolIcons";
#endif

			FieldInfo toolIcons = toolbarType.GetField(fieldName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

#if UNITY_2019_3_OR_NEWER
			m_toolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 8;
#elif UNITY_2019_1_OR_NEWER
			m_toolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 7;
#elif UNITY_2018_1_OR_NEWER
			m_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 6;
#else
			m_toolCount = toolIcons != null ? ((Array)toolIcons.GetValue(null)).Length : 5;
#endif

			ToolbarCallback.OnToolbarGUI -= OnGUI;
			ToolbarCallback.OnToolbarGUI += OnGUI;

			setting = CustomToolbarSetting.GetOrCreateSetting();

			foreach (var element in setting.elements)
				element.Init();
		}

		public static void OnGUI()
		{
			// Create two containers, left and right
			// Screen is whole toolbar

			if (m_commandStyle == null)
			{
				m_commandStyle = new GUIStyle("CommandLeft");
			}

			var screenWidth = EditorGUIUtility.currentViewWidth;

			// Following calculations match code reflected from Toolbar.OldOnGUI()
			float playButtonsPosition = (screenWidth - 180) / 2;

			Rect leftRect = new Rect(0, 0, screenWidth, Screen.height);
			leftRect.xMin += 10; // Spacing left
			leftRect.xMin += 32 * m_toolCount; // Tool buttons
			leftRect.xMin += 20; // Spacing between tools and pivot
			leftRect.xMin += 64 * 2; // Pivot buttons
			leftRect.xMax = playButtonsPosition;

			Rect rightRect = new Rect(0, 0, screenWidth, Screen.height);
			rightRect.xMin = playButtonsPosition + 50;
			rightRect.xMin += m_commandStyle.fixedWidth * 3; // Play buttons
			rightRect.xMax = screenWidth;
			rightRect.xMax -= 10; // Spacing right
			rightRect.xMax -= 80; // Layout
			rightRect.xMax -= 10; // Spacing between layout and layers
			rightRect.xMax -= 80; // Layers
			rightRect.xMax -= 20; // Spacing between layers and account
			rightRect.xMax -= 80; // Account
			rightRect.xMax -= 10; // Spacing between account and cloud
			rightRect.xMax -= 32; // Cloud
			//rightRect.xMax -= 10; // Spacing between cloud and collab
			//rightRect.xMax -= 78; // Colab

			// Add spacing around existing controls
			leftRect.xMin -= 10;
			leftRect.xMax += 10;
			rightRect.xMin -= 20;
			rightRect.xMax += 10;

			// Add top and bottom margins
			leftRect.y = 5;
			leftRect.height = 24;
			rightRect.y = 5;
			rightRect.height = 24;

			if (leftRect.width > 0)
			{
				GUILayout.BeginArea(leftRect);
				GUILayout.BeginHorizontal();

				GUILayout.FlexibleSpace();

				for (int i = 0; i < setting.elements.Count; ++i) {
					if (setting.elements[i] is ToolbarSides)
						break;
					setting.elements[i].DrawInToolbar();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

			if (rightRect.width > 0)
			{
				GUILayout.BeginArea(rightRect);
				GUILayout.BeginHorizontal();

				int i = 0;
				for (; i < setting.elements.Count; ++i)
					if (setting.elements[i] is ToolbarSides)
						break;
				for (++i; i < setting.elements.Count; ++i)
					setting.elements[i].DrawInToolbar();

				GUILayout.FlexibleSpace();

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
	}
}
