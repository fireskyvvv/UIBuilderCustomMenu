using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UIBuilderCustomMenu.Editor
{
    internal static class UIBuilderCustomMenuCreator
    {
        public class Creator
        {
            public readonly Func<List<CustomMenuInfo>> Create;
            public readonly int Priority;

            public Creator(Func<List<CustomMenuInfo>> create, int priority)
            {
                Create = create;
                Priority = priority;
            }
        }

        // EditorWindowの更新を検出し、UIBuilderが開かれたタイミングでカスタムを実行する
        private static readonly FieldInfo WindowsReorderedField = typeof(EditorApplication).GetField(
            "windowsReordered",
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic
        );

        private static EditorApplication.CallbackFunction _windowsReordered;

        private static EditorApplication.CallbackFunction WindowsReordered
        {
            get
            {
                if (_windowsReordered == null)
                {
                    _windowsReordered = WindowsReorderedField.GetValue(null) as EditorApplication.CallbackFunction;
                }

                return _windowsReordered;
            }
            set
            {
                var functions = WindowsReorderedField.GetValue(null) as EditorApplication.CallbackFunction;
                functions += value;
                WindowsReorderedField.SetValue(null, functions);
            }
        }

        private static PropertyInfo _activeWindowProp;

        private static PropertyInfo ActiveWindowProp
        {
            get
            {
                if (_activeWindowProp != null)
                {
                    return _activeWindowProp;
                }

                var builderType = Type.GetType("Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule");
                if (builderType == null)
                {
                    UnityEngine.Debug.LogError("Cant get type of UIBuilder");
                    return null;
                }

                var activeWindowProp =
                    builderType.GetProperty("ActiveWindow",
                        BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
                    );

                if (activeWindowProp == null)
                {
                    UnityEngine.Debug.LogError("Cant get prop of ActiveWindow");
                    return null;
                }

                _activeWindowProp = activeWindowProp;

                return _activeWindowProp;
            }
        }

        private static EditorWindow _currentActiveUIBuilderWindow;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            CustomMenuCollector.CollectCustomMenuCreateMethods();
            WindowsReordered += AddCustomToolbar;

            // コンパイル時にUIBuilderが表示中の場合にCustomMenuを追加する
            EditorApplication.delayCall += InitCurrentActiveUIBuilder;

            return;

            void InitCurrentActiveUIBuilder()
            {
                EditorApplication.delayCall -= InitCurrentActiveUIBuilder;
                AddCustomToolbar();
            }
        }

        public static void Refresh()
        {
            _currentActiveUIBuilderWindow = null;
            AddCustomToolbar();
        }

        private static void AddCustomToolbar()
        {
            if (_currentActiveUIBuilderWindow != null)
            {
                return;
            }

            var activeWindow = ActiveWindowProp.GetMethod.Invoke(null, null) as EditorWindow;

            if (activeWindow == null)
            {
                return;
            }

            var creators = CustomMenuCollector.Creators;
            if (creators.Count == 0)
            {
                return;
            }

            _currentActiveUIBuilderWindow = activeWindow;

            var activeWindowRootVisualElement = activeWindow.rootVisualElement;
            var unityBuilderToolbar = activeWindowRootVisualElement.Q(className: "unity-builder-toolbar");
            var canvasThemeMenu = unityBuilderToolbar.Q(name: "canvas-theme-menu");

            const string toolbarMenuName = "CustomMenuToolbarMenu";

            var customMenusToolbarMenu = unityBuilderToolbar.Q<ToolbarMenu>(toolbarMenuName);
            if (customMenusToolbarMenu == null)
            {
                customMenusToolbarMenu = new ToolbarMenu()
                {
                    name = toolbarMenuName,
                    text = "Custom Menus"
                };

                var canvasThemeMenuIndex = unityBuilderToolbar.hierarchy.IndexOf(canvasThemeMenu);
                if (canvasThemeMenuIndex == -1)
                {
                    UnityEngine.Debug.LogError($"canvas-theme-menu is not found");
                    return;
                }

                // CanvasThemeMenuの左横に生成
                unityBuilderToolbar.hierarchy.Insert(canvasThemeMenuIndex, customMenusToolbarMenu);
            }

            customMenusToolbarMenu.menu.ClearItems();

            customMenusToolbarMenu.menu.AppendAction(
                actionName: "Refresh",
                action: _ => { Refresh(); }
            );

            var orderedCreators = creators.OrderBy(x => x.Priority).ToList();

            if (orderedCreators.Count == 0)
            {
                return;
            }

            customMenusToolbarMenu.menu.AppendSeparator();

            // Priority順に並び替え
            var prevPriority = orderedCreators[0].Priority;
            foreach (var customMenusCreator in orderedCreators)
            {
                // MenuItemと同様にPriorityが10離れている場合にセパレータを入れる
                if (customMenusCreator.Priority - prevPriority > 10)
                {
                    customMenusToolbarMenu.menu.AppendSeparator();
                }

                var customMenus = customMenusCreator.Create.Invoke();
                foreach (var customMenu in customMenus)
                {
                    var menuPath = customMenu.MenuPath;
                    customMenusToolbarMenu.menu.AppendAction(
                        actionName: menuPath,
                        action: customMenu.OnClickAction,
                        actionStatusCallback: customMenu.GetStatus
                    );

                    if (customMenu.Separate)
                    {
                        customMenusToolbarMenu.menu.AppendSeparator(menuPath[..(menuPath.LastIndexOf('/') + 1)]);
                    }
                }

                prevPriority = customMenusCreator.Priority;
            }
        }
    }
}