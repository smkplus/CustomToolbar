using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender
{
    
    [InitializeOnLoad]
    public static class RestartPlayMode
    {

        static RestartPlayMode()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }
        static void OnToolbarGUI()
        {

            
            EditorGUIUtility.SetIconSize(new Vector2(17,17));
            if (GUILayout.Button(EditorGUIUtility.IconContent("LookDevResetEnv@2x"), ToolbarStyles.commandButtonStyle))
            {
                if (EditorApplication.isPlaying)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            
            Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, 1, 20,GUILayout.Width(150));

        }
    }
}