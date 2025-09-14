#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ConvertLegacyClipsToMecanim
{
    static readonly string[] Targets = {
        "Glare_Flick","IdleOpen","Activation","Skill_activation","Deactivation",
        "IdleClose","Kick","Dribling_Card"
    };

    [MenuItem("Tools/Fix/Convert Legacy Clips To Mecanim")]
    public static void Run()
    {
        foreach (var name in Targets)
        {
            string[] guids = AssetDatabase.FindAssets($"t:AnimationClip {name}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if (!clip) continue;

                var so = new SerializedObject(clip);
                var legacy = so.FindProperty("m_Legacy");
                if (legacy != null && legacy.boolValue)
                {
                    legacy.boolValue = false;
                    so.ApplyModifiedProperties();
                    Debug.Log($"Converted legacy OFF: {path}", clip);
                }
            }
        }
    }
}
#endif
