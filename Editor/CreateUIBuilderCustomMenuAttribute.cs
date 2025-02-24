using System;

namespace UIBuilderCustomMenu.Editor
{
    /// <summary>
    /// このAttributeがついた関数からカスタムメニューに表示する内容を構築します。
    /// 関数は引数を持たないstaticな関数である必要があります。
    /// List&lt;<see cref="CustomMenuInfo"/>&gt;である必要があります。
    /// Priorityが11以上離れている間にはセパレータが表示されます。
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class CreateUIBuilderCustomMenuAttribute : Attribute
    {
        public int Priority { get; }

        public CreateUIBuilderCustomMenuAttribute(int priority = 10)
        {
            Priority = priority;
        }
    }
}