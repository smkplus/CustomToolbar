using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

internal static class CustomToolbarReordableList {
	internal static ReorderableList Create(List<BaseToolbarElement> configsList, GenericMenu.MenuFunction2 menuItemHandler) {
		var reorderableList = new ReorderableList(configsList, typeof(BaseToolbarElement), true, false, true, true);

		reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 4;
		reorderableList.drawElementCallback = (position, index, isActive, isFocused) => {
			configsList[index].DrawInList(position);
		};

		reorderableList.onAddDropdownCallback = (buttonRect, list) => {
			//TODO: add items without changing this file
			// Probably reflection can helps
			BaseToolbarElement[] elements = new BaseToolbarElement[] {
				new ToolbarSides(),
				new ToolbarSpace(),
				null,

				new ToolbarTimeslider(),
				new ToolbarFPSSlider(),
				null,

				new ToolbarSceneSelection(),
				new ToolbarEnterPlayMode(),
				null,

				new ToolbarReloadScene(),
				new ToolbarStartFromFirstScene(),
				null,

				new ToolbarSavingPrefs(),
				new ToolbarClearPrefs(),
				null,

				new ToolbarRecompile(),
				new ToolbarReserializeSelected(),
				new ToolbarReserializeAll(),
				null,
			};

			GenericMenu menu = new GenericMenu();

			foreach (var element in elements) {
				if(element == null) 
					menu.AddSeparator("");
				else 
					menu.AddItem(new GUIContent(element.NameInList), false, menuItemHandler, element);
			}

			menu.ShowAsContext();
		};

		return reorderableList;
	}
}
