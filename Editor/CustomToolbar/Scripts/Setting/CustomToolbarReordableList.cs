using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;
using System.Linq;

internal static class CustomToolbarReordableList {
	internal static ReorderableList Create(List<BaseToolbarElement> configsList, GenericMenu.MenuFunction2 menuItemHandler) {
		var reorderableList = new ReorderableList(configsList, typeof(BaseToolbarElement), true, false, true, true);

		reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 4;
		reorderableList.drawElementCallback = (position, index, isActive, isFocused) => {
			configsList[index].DrawInList(position);
		};

		reorderableList.onAddDropdownCallback = (buttonRect, list) => {

			BaseToolbarElement[] elements = GetNewObjectsOfType<BaseToolbarElement>().ToArray();

			GenericMenu menu = new GenericMenu();

            int lastSortingGroup = 0;
			foreach (var element in elements)
			{
                if (lastSortingGroup != element.SortingGroup)
                {
                    menu.AddSeparator("");
                    lastSortingGroup = element.SortingGroup;
                }
                
		        menu.AddItem(new GUIContent(element.NameInList), false, menuItemHandler, element);
			}

			menu.ShowAsContext();
		};

		return reorderableList;
	}

    //  Get the possible menu options. Generates the cache if it needs to
    private static List<T> GetNewObjectsOfType<T>() where T : class, IComparable<T>
    {
        if (toolbarOptions == null)
        {
            toolbarOptions = FindConstructors<T>();
        }

        List<T> objects = new List<T>();

        //  Create each object using the data from the cache
        foreach (ConstructorOption item in toolbarOptions)
        {
            objects.Add((T)item.constructor.Invoke(item.parameters));
        }

        objects.Sort();

        return objects;
    }

    //  Finds all the toolbar elements, and caches their constructors and parameters
    private static ConstructorOption[] FindConstructors<T>() where T : class, IComparable<T>
    {
        List<ConstructorOption> typeConstructorList = new List<ConstructorOption>();

        //  Loop over every type in the assembly
        foreach (Type type in
            Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            //  Get the default constructor
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            object[] constructorParameters = null;

            //  If we cant find a default constructor, get all the constructors, and try to find an alternative
            if (constructor == null)
            {
                ConstructorInfo[] constructors = type.GetConstructors();

                foreach (var item in constructors)
                {
                    //  Find a constructor that has all optional parameters, then use the default parameter values
                    ParameterInfo[] parameters = item.GetParameters();
                    object[] tempParameters = new object[parameters.Length];

                    int validParameters = 0;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ParameterInfo parameter = parameters[i];
                        object defaultValue = parameter.RawDefaultValue;

                        //  This is to check that it actually has a default parameter value
                        if (defaultValue != DBNull.Value)
                        {
                            validParameters++;
                            tempParameters[i] = defaultValue;
                        }
                    }

                    if (validParameters == parameters.Length)
                    {
                        constructor = item;
                        constructorParameters = tempParameters;
                    }
                }
            }

            //  If we have found a suitable constructor, then add it to the list of possible types 
            if (constructor != null)
            {
                typeConstructorList.Add(new ConstructorOption(constructor, constructorParameters));
            }
            else
            {
                Debug.LogWarning("Custom Toolbar was unable to find a suitable constructor for the class " + type.ToString() +
                    ". To show up in the Project Settings it must either contain a default constructor (no parameters), or a constructor where all parameters have a default value.");
            }
        }

        return typeConstructorList.ToArray();
    }

    //  This is to cache the possible toolbar options
    private static ConstructorOption[] toolbarOptions = null;

    private struct ConstructorOption
    {
        public ConstructorInfo constructor;
        public object[] parameters;

        public ConstructorOption(ConstructorInfo constructor, object[] parameters)
        {
            this.constructor = constructor;
            this.parameters = parameters;
        }
    }
}
