using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender
{
	internal class CustomToolbarSetting : ScriptableObject
	{
		const string SETTING_PATH = "Assets/Editor/Setting/CustomToolbarSetting.asset";
		
		[SerializeField] internal int minFPS = 1;
		[SerializeField] internal int maxFPS = 120;
		[SerializeField] internal bool limitFPS = true;
		[SerializeField] internal string m_SomeString;

		internal static CustomToolbarSetting GetOrCreateSetting()
		{
			var setting = AssetDatabase.LoadAssetAtPath<CustomToolbarSetting>(SETTING_PATH);
			if (setting == null)
			{
				setting = ScriptableObject.CreateInstance<CustomToolbarSetting>();
				setting.minFPS = 1;
				setting.maxFPS = 120;
				setting.limitFPS = true;

				if (!Directory.Exists("Assets/Editor"))
				{
					AssetDatabase.CreateFolder("Assets", "Editor");
					AssetDatabase.SaveAssets();
					
					AssetDatabase.CreateFolder("Assets/Editor", "Setting");
					AssetDatabase.SaveAssets();
				}
				else
				{
					if (!Directory.Exists("Assets/Editor/Setting"))
					{
						AssetDatabase.CreateFolder("Assets/Editor", "Setting");
						AssetDatabase.SaveAssets();
					}
				}

				AssetDatabase.CreateAsset(setting, SETTING_PATH);
				AssetDatabase.SaveAssets();
			}

			return setting;
		}

		internal static SerializedObject GetSerializedSetting()
		{
			return new SerializedObject(GetOrCreateSetting());
		}
	}
}