#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;

namespace ZombieSoccer.GameLayer.Characters
{
    [CreateAssetMenu(fileName = "New character", menuName = "Character", order = 51)]
    public class EditorCharacterScriptableObject : ScriptableObject
    {
        [PreviewField]
        public Sprite icon;

        [OnValueChanged("UpdateCharacterId")]
        public Character character;

        private void UpdateCharacterId()
        {
            character.archetypeId = Mathf.Abs(character.GetHashCode()).ToString();
            Debug.LogWarning($"[UpdateCharacterId] archetype id: {character.archetypeId}");
        }

        private void UpdateCharacterIcon()
        {
            var path = Path.Combine("Assets", "Resources", "Characters", "Icons", $"{character.archetypeId}.png");
            var result = UnityEditor.AssetDatabase.CopyAsset(UnityEditor.AssetDatabase.GetAssetPath(icon), path);
            Debug.LogWarning($"[UpdateCharacterIcon] Result: {result}; Path: {path}");
        }

        [Button]
        public void LoadJson(string data)
        {
            character = JsonConvert.DeserializeObject<Character>(data);
        }

        [Button]
        public string BuildCharacter()
        {
            UpdateCharacterId();
            UpdateCharacterIcon();
            RenameFile();

            var data = JsonConvert.SerializeObject(character);
            Debug.LogWarning($"[Character built] Character:\n{data}");

            return data;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(character);
        }

        private void RenameFile()
        {
            string thisFileNewName = character.archetypeId;
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
                UnityEditor.AssetDatabase.RenameAsset(assetPath, thisFileNewName);
        }
    }
}
#endif