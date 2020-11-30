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
			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Left-Right side splitter"), false, menuItemHandler, new ToolbarSides());
			menu.AddItem(new GUIContent("Space"), false, menuItemHandler, new ToolbarSpace());
			menu.AddSeparator("");

			menu.AddItem(new GUIContent("Time slider"), false, menuItemHandler, new ToolbarTimeslider());
			menu.AddItem(new GUIContent("FPS slider"), false, menuItemHandler, new ToolbarFPSSlider());
			menu.AddSeparator("");

			menu.AddItem(new GUIContent("ToolbarSceneSelection"), false, menuItemHandler, new ToolbarSceneSelection());
			menu.AddItem(new GUIContent("ToolbarEnterPlayMode"), false, menuItemHandler, new ToolbarEnterPlayMode());
			menu.AddSeparator("");

			menu.AddItem(new GUIContent("ToolbarReloadScene"), false, menuItemHandler, new ToolbarReloadScene());
			menu.AddItem(new GUIContent("ToolbarStartFromFirstScene"), false, menuItemHandler, new ToolbarStartFromFirstScene());
			menu.AddSeparator("");

			menu.AddItem(new GUIContent("ToolbarSavingPrefs"), false, menuItemHandler, new ToolbarSavingPrefs());
			menu.AddItem(new GUIContent("ToolbarClearPrefs"), false, menuItemHandler, new ToolbarClearPrefs());
			menu.AddSeparator("");

			menu.AddItem(new GUIContent("ToolbarRecompile"), false, menuItemHandler, new ToolbarRecompile());
			menu.AddItem(new GUIContent("ToolbarReserializeSelected"), false, menuItemHandler, new ToolbarReserializeSelected());
			menu.AddItem(new GUIContent("ToolbarReserializeAll"), false, menuItemHandler, new ToolbarReserializeAll());
			menu.AddSeparator("");

			menu.ShowAsContext();
		};

		return reorderableList;
	}
}
