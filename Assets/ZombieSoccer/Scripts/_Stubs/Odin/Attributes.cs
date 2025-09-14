#if !ODIN_INSPECTOR
namespace Sirenix.OdinInspector {
  using System;
  public class ButtonAttribute : Attribute { public ButtonAttribute(){} }
  public class FoldoutGroupAttribute : Attribute { public FoldoutGroupAttribute(string n){} }
  public class ShowIfAttribute : Attribute { public ShowIfAttribute(string c){} }
  public class ReadOnlyAttribute : Attribute {}
  public class PreviewFieldAttribute : Attribute {}
  public class PropertyOrderAttribute : Attribute { public PropertyOrderAttribute(int o){} }
  public class InfoBoxAttribute : Attribute { public InfoBoxAttribute(string m){} }
  public class OnValueChangedAttribute : Attribute { public OnValueChangedAttribute(string m){} }
  public class HideReferenceObjectPickerAttribute : Attribute {}
  public class ListDrawerSettingsAttribute : Attribute {
    public string CustomAddFunction; public string OnTitleBarGUI;
  }
}
#endif
