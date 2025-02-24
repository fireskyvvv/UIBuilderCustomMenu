using System;
using UnityEngine.UIElements;

namespace UIBuilderCustomMenu.Editor
{
    /// <summary>
    /// Defines information about the CustomMenu to be created
    /// </summary>
    public class CustomMenuInfo
    {
        public readonly string MenuPath;
        public readonly Action<DropdownMenuAction> OnClickAction;
        public readonly Func<DropdownMenuAction, DropdownMenuAction.Status> GetStatus;
        public readonly bool Separate;

        /// <param name="menuPath">CustomMenu path</param>.
        /// <param name="onClickAction">Callback when the menu is pressed</param>.
        /// <param name="getStatus">Callback that returns the menu status</param>.
        /// <param name="separate">Whether the separator is displayed</param>.
        public CustomMenuInfo(
            string menuPath,
            Action<DropdownMenuAction> onClickAction,
            Func<DropdownMenuAction, DropdownMenuAction.Status> getStatus = null,
            bool separate = false
        )
        {
            MenuPath = menuPath;
            OnClickAction = onClickAction;
            GetStatus = getStatus ?? (_ => DropdownMenuAction.Status.Normal);
            Separate = separate;
        }
    }
}