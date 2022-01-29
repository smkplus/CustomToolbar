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
            CustomToolbarSetting setting = CustomToolbarSetting.GetOrCreateSetting();

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