using UnityEditor; using UnityEngine;
public static class LegacyToMecanimFixer {
  [MenuItem("ZSC/Fix Legacy Animation Clips")]
  public static void Fix() {
    var guids = AssetDatabase.FindAssets("t:AnimationClip");
    foreach (var guid in guids) {
      var path = AssetDatabase.GUIDToAssetPath(guid);
      var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
      if (clip == null) continue;
      var so = new SerializedObject(clip);
      var prop = so.FindProperty("m_Legacy");
      if (prop != null && prop.boolValue) {
        prop.boolValue = false;
        so.ApplyModifiedProperties();
        Debug.Log($"Converted to non-legacy: {path}");
      }
    }
    AssetDatabase.SaveAssets();
  }
}
