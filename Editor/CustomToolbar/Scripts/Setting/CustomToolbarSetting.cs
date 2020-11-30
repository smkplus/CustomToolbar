using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender
{
	internal class CustomToolbarSetting : ScriptableObject
	{
		const string SETTING_PATH = "Assets/Editor/Setting/CustomToolbarSetting.asset";

		[SerializeReference] internal List<BaseToolbarElement> elements = new List<BaseToolbarElement>();

		internal static CustomToolbarSetting GetOrCreateSetting()
		{
			var setting = AssetDatabase.LoadAssetAtPath<CustomToolbarSetting>(SETTING_PATH);
			if (setting == null)
			{
				setting = ScriptableObject.CreateInstance<CustomToolbarSetting>();

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

				foreach (var element in setting.elements) {
					element.Init();
				}
			}

			return setting;
		}

		internal static SerializedObject GetSerializedSetting()
		{
			return new SerializedObject(GetOrCreateSetting());
		}
	}
}