using System;
using UnityEngine.UIElements;

namespace UIBuilderCustomMenu.Editor
{
    /// <summary>
    /// 作成されるCustomMenuに関する情報を定義します
    /// </summary>
    public class CustomMenuInfo
    {
        public readonly string MenuPath;
        public readonly Action<DropdownMenuAction> Action;
        public readonly Func<DropdownMenuAction, DropdownMenuAction.Status> GetStatus;
        public readonly bool Separate;
        
        /// <param name="menuPath">CustomMenuのパス</param>
        /// <param name="callback">メニューを押下したときのコールバック</param>
        /// <param name="getStatus">メニューの状態を返すコールバック</param>
        /// <param name="separate">セパレータを表示するかどうか</param>
        public CustomMenuInfo(
            string menuPath,
            Action<DropdownMenuAction> callback,
            Func<DropdownMenuAction, DropdownMenuAction.Status> getStatus = null,
            bool separate = false
        )
        {
            MenuPath = menuPath;
            Action = callback;
            GetStatus = getStatus ?? (_ => DropdownMenuAction.Status.Normal);
            Separate = separate;
        }
    }
}