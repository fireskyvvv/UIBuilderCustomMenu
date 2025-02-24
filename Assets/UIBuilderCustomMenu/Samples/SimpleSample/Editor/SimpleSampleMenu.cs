using System.Collections.Generic;
using UIBuilderCustomMenu.Editor;

namespace UIBuilderCustomMenu.Samples.SimpleSample.Editor
{
    internal class SimpleSampleMenu
    {
        [CreateUIBuilderCustomMenu(priority:30)]
        private static List<CustomMenuInfo> Create()
        {
            return new List<CustomMenuInfo>()
            {
                new(
                    menuPath: $"Test1",
                    onClickAction: _ => { UnityEngine.Debug.Log("Test1 Clicked!"); }
                ),
                new(
                    menuPath: $"Test2",
                    onClickAction: _ => { UnityEngine.Debug.Log("Test2 Clicked!"); }
                ),
                new(
                    menuPath: $"Test3",
                    onClickAction: _ => { UnityEngine.Debug.Log("Test2 Clicked!"); },
                    separate: true
                ),
                new(
                    menuPath: $"Sub/1",
                    onClickAction: _ => { UnityEngine.Debug.Log("Sub/1 Clicked!"); }
                ),
                new(
                    menuPath: $"Sub/2",
                    onClickAction: _ => { UnityEngine.Debug.Log("Sub/2 Clicked!"); },
                    separate: true
                ),
                new(
                    menuPath: $"Sub/3",
                    onClickAction: _ => { UnityEngine.Debug.Log("Sub/3 Clicked!"); }
                ),
            };
        }
    }
}