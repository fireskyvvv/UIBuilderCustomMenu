# UIBuilderCustomMenu

これはUIToolKitの [UIBuilder](https://docs.unity3d.com/6000.0/Documentation/Manual/UIB-interface-overview.html) 上にカスタムしたメニューを表示するEditor拡張です。

## Demo

![ui-builder-custom-menu-demo](https://github.com/user-attachments/assets/28cbb6e7-9ed7-4f93-b6af-3c581d423053)

## Installation

- Install
  - UPM
    - Add https://github.com/fireskyvvv/UIBuilderCustomMenu.git#upm to Unity Package Manager

## メニューの追加方法

`[CreateUIBuilderCustomMenuAttribute]` がついたメソッドから自動的にメニューを作成し、UIBuilder上の `Custom Menu` 以下に追加します。  

例:
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

UIBuilderCustomMenuは `[CreateUIBuilderCustomMenuAttribute]` が設定されたメソッドからメニューを作成します。  
`[CreateUIBuilderCustomMenuAttribute]` を設定することができるメソッドは以下の条件である必要があります。  

- static method
- 引数を持たない
- List<CustomMenuInfo> を返り値に持つ

### `Priority`

Priorityは各Menuを表示する順序を決定するのに使われます。    
数値が小さいほどより優先的に表示されます。  

また、Priorityの差が11以上ある場合、セパレータが表示されます。

## `CustomMenuInfo`

`CustomMenuInfo` はメニューのパスやクリックした時の挙動を設定することができます。

### `MenuPath` 

`MenuPath`に設定された文字列に従ってUIBuilder上にメニューを追加します。  
`Sub/1` という文字列が設定された場合、UIBuilder上で `Custom Menu > Sub > 1` とメニューが追加されます。

### `OnClickAction`

メニューがクリックされた場合の挙動を定義することができます。  

### `GetStatus`

メニューのステータスの状態を取得するFuncを指定することができます。  
返り値は `DropdownMenuAction.Status` です。  
詳細は以下を参照してください。  
https://docs.unity3d.com/6000.0/Documentation/ScriptReference/UIElements.DropdownMenuAction-status.html

### `Separate`

メニュー間にセパレータを表示するかどうかを決定します。






