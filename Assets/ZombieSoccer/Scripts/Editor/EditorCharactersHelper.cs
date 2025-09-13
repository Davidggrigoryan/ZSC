#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZombieSoccer.GameLayer.Characters;

namespace ZombieSoccer.Editor
{
    [Serializable]
    public class EditorCharactersHelper : MonoBehaviour
    {
        //[NonSerialized]
        public List<EditorCharacterScriptableObject> peopleCharacters, zombiesCharacters;

        [Button]
        public string BuildJsonFromCharactersCollection()
        {
            Dictionary<string, Character> People = new Dictionary<string, Character>();
            Dictionary<string, Character> Zombies = new Dictionary<string, Character>();

            foreach (var character in peopleCharacters)
                People.Add(character.character.archetypeId, character.character);

            foreach (var character in zombiesCharacters)
                Zombies.Add(character.character.archetypeId, character.character);

            return JsonConvert.SerializeObject(new CharactersData() { people = People, zombies = Zombies });
        }

        [Button]
        public void CreateFolders(string str)
        {
            var data = str.Split(' ');
            var path = Application.dataPath + "/" + "Resources/Characters/";
            Debug.LogError(path);
            foreach (var i in data)
            {
                Directory.CreateDirectory(path + i);
            }
        }

        //[Button]
        //void TestData()
        //{
        //    Debug.Log(CharacterManager.chractersData.people.Count);
        //    foreach(var character in CharacterManager.chractersData.people)
        //        Debug.Log(character.ToString());

        //    Debug.Log(CharacterManager.chractersData.zombies.Count);

        //    foreach (var character in CharacterManager.chractersData.zombies)
        //        Debug.Log(character.ToString());
        //}


    }
}
#endif