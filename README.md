# UIBuilderCustomMenu

This is an Editor extension that displays custom menus on Unity's [UIBuilder](https://docs.unity3d.com/6000.0/Documentation/Manual/UIB-interface-overview.html).

## Installation

- Install
  - UPM
    - Add https://github.com/fireskyvvv/UIBuilderCustomMenu.git#upm to Unity Package Manager

## How to Add Menus

Menus are automatically created from methods with the `[CreateUIBuilderCustomMenuAttribute]` attribute and added under the `Custom Menu` section in UIBuilder.

Example:
```csharp
internal class SimpleSampleMenu
{
    [CreateUIBuilderCustomMenu(priority:30)]
    private static List<CustomMenuInfo> Create()
    {
        return new List<CustomMenuInfo>()
        {
            new(
                menuPath: $"Test1",
                callback: _ => { UnityEngine.Debug.Log("Test1 Clicked!"); }
            ),
        }
    }
}
```

## `[CreateUIBuilderCustomMenuAttribute]`

UIBuilderCustomMenu creates menus from methods that have the `[CreateUIBuilderCustomMenuAttribute]` attribute.  
Methods with this attribute must satisfy the following conditions:

- Must be static methods
- Must not have any parameters
- Must return List<CustomMenuInfo>

### `Priority`

Priority is used to determine the display order of each menu.  
Lower values are displayed with higher priority.

Additionally, if the difference in priority is 11 or more, a separator will be displayed.

## `CustomMenuInfo`

`CustomMenuInfo` allows you to set the menu path and behavior when clicked.

### `MenuPath`

Menus are added to UIBuilder according to the string set in `MenuPath`.  
If the string `Sub/1` is set, the menu will appear in UIBuilder as `Custom Menu > Sub > 1`.

### `OnClickAction`

Defines the behavior when a menu is clicked.

### `GetStatus`

You can specify a Func to retrieve the status of the menu.  
The return value is `DropdownMenuAction.Status`.  
For details, please refer to:  
https://docs.unity3d.com/6000.0/Documentation/ScriptReference/UIElements.DropdownMenuAction-status.html

### `Separate`

Determines whether to display a separator between menus.
