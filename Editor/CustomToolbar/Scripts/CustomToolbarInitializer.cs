using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UnityToolbarExtender
{
    [InitializeOnLoad]
    public static class CustomToolbarInitializer
    {
        static CustomToolbarInitializer()
        {
#if UNITY_2020_3_OR_NEWER
            CustomToolbarSetting setting = ScriptableSingleton<CustomToolbarSetting>.instance;
#else
            CustomToolbarSetting setting = CustomToolbarSetting.GetOrCreateSetting();
#endif

            setting.elements.ForEach(element => element.Init());

            List<BaseToolbarElement> leftTools = setting.elements.TakeWhile(element => !(element is ToolbarSides)).ToList();
            List<BaseToolbarElement> rightTools = setting.elements.Except(leftTools).ToList();
            IEnumerable<Action> leftToolsDrawActions = leftTools.Select(TakeDrawAction);
            IEnumerable<Action> rightToolsDrawActions = rightTools.Select(TakeDrawAction);

            ToolbarExtender.LeftToolbarGUI.AddRange(leftToolsDrawActions);
            ToolbarExtender.RightToolbarGUI.AddRange(rightToolsDrawActions);
        }

        private static Action TakeDrawAction(BaseToolbarElement element)
        {
            Action action = element.DrawInToolbar;
            return action;
        }
    }
}