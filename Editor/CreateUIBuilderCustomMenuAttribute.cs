using System;

namespace UIBuilderCustomMenu.Editor
{
    /// <summary>
    /// The content to be displayed in the custom menu is constructed from the function with this Attribute.<br/>
    /// The function must be a static function with no arguments.<br/>
    /// Return type must be List&lt;<see cref="CustomMenuInfo"/>&gt;<br/>
    /// Separator is displayed while Priority is more than 11 apart.<br/>
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