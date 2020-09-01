using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender
{
    
    [InitializeOnLoad]
    public static class CustomToolbarRight
    {
        static CustomToolbarRight()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, 0f, 2f, GUILayout.Width(300));
        }
    }
}