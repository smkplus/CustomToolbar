using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }
    

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        private static bool _deleteKeys;

        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            
            if (_deleteKeys)
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("SavePassive"), ToolbarStyles.commandButtonStyle)) _deleteKeys = false;
            }
            else
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("SaveActive"), ToolbarStyles.commandButtonStyle)) _deleteKeys = true;
            }
 

            
            if (GUILayout.Button(EditorGUIUtility.IconContent("SaveFromPlay"), ToolbarStyles.commandButtonStyle))
            {
                PlayerPrefs.DeleteAll();
            }

            if (_deleteKeys)
            {
                PlayerPrefs.DeleteAll();
            }

            if (GUILayout.Button(new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/LookDevSingle1@2x.png", typeof(Texture2D))), ToolbarStyles.commandButtonStyle))
            {
                if (!EditorApplication.isPlaying)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorPrefs.SetInt("LastActiveScene", EditorSceneManager.GetActiveScene().buildIndex);
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
                }

                EditorApplication.isPlaying = !EditorApplication.isPlaying;
            }
            

        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && EditorPrefs.HasKey("LastActiveScene"))
            {
                EditorSceneManager.OpenScene(
                    SceneUtility.GetScenePathByBuildIndex(EditorPrefs.GetInt("LastActiveScene")));
                EditorPrefs.DeleteKey("LastActiveScene");
            }
        }
    }
}