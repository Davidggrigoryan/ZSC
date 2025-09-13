//using System;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;
//using Zenject;
////using ZombieSoccer.Data;
//using ZombieSoccer.GameLayer.Characters;
//using ZombieSoccer.GameLayer.Flow;
//using ZombieSoccer.GameLayer.Matching.Core;
//using ZombieSoccer.GameLayer.Skills;
//using ZombieSoccer.GameLayer.UI;

//namespace ZombieSoccer.GameLayer.Matching
//{
//    public class UIMatchField
//    {
//        //[Inject]
//        //MatchLogger matchLogger;

//        [Inject]
//        GameMatchField matchMap;

//        [Inject]
//        DetailCharacterView.Factory detailCharacterViewFactory;

//        [Inject]
//        MinimalCharacterView.Factory minimalCharacterViewFactory;

//        [Inject]
//        CharactersManager CharacterManager;

//        [Inject]
//        SkillsManager skillsManager;

//        public static event Action<bool> OnValidatePoleEvent;

//        public static int2 poleSize = new int2(3, 5);

//        Character allyGoalkeeper, enemyGoalkeeper;

//        [SerializeField]
//        public Cell[,] cellsViewArray = new Cell[poleSize.x, poleSize.y];

//        public void AddCell(Cell cell)
//        {
//            cellsViewArray[cell.cellPosition.x, cell.cellPosition.y] = cell;
//        }

//        public void OnStartMatch()
//        {
//            ConvertPoleToArray();

//            allyGoalkeeper = cellsViewArray[1, 0].allyCharacter;
//            enemyGoalkeeper = cellsViewArray[1, 4].enemyCharacter;
//        }

//        private void ConvertPoleToArray()
//        {
//            matchMap.Clear();
//            //matchLogger.Log("--------------------------------------------------------------------------------");
//            //matchLogger.Log();

//            for (int i = 0; i < poleSize.y; i++)
//            {
//                List<Character> alliesTemp = new List<Character>();
//                List<Character> enemiesTemp = new List<Character>();

//                for (int j = 0; j < poleSize.x; j++)
//                {
//                    if (cellsViewArray[j, i].allyCharacter != null)
//                    {
//                        alliesTemp.Add(cellsViewArray[j, i].allyCharacter);
//                        //matchLogger.Log($"Character team: {TeamType.Ally.ToString()}; Position: {i}");
//                        //matchLogger.Log($"{cellsViewArray[j, i].allyCharacter.ToString()}");
//                        //matchLogger.Log();
//                    }

//                    if (cellsViewArray[j, i].enemyCharacter != null)
//                    {
//                        enemiesTemp.Add(cellsViewArray[j, i].enemyCharacter);
//                        //matchLogger.Log($"Character team: {TeamType.Enemy.ToString()}; Position: {i}");
//                        //matchLogger.Log($"{cellsViewArray[j, i].enemyCharacter.ToString()}");
//                        //matchLogger.Log();
//                    }
//                }

//                var tempSegmentData = new Dictionary<TeamType, List<Character>>()
//                {
//                    { TeamType.Ally, alliesTemp},
//                    { TeamType.Enemy, enemiesTemp}
//                };

//                matchMap.SetSegmentData(i, tempSegmentData);
//            }

//            //matchLogger.Log("--------------------------------------------------------------------------------");
//            //matchLogger.Log();
//        }

//        public List<Character>[] alliesOnField, enemiesOnField;

//        //[Inject]
//        //Settings.MatchSettings matchSettings;

//        public float viewOffset = 45f;

//        public void AddAllies(List<CharacterPosition> data)
//        {
//            //alliesOnField = new List<Character>[6];
//            //for (int i = 0; i < alliesOnField.Length; i++)
//            //    alliesOnField[i] = new List<Character>();
                

//            //foreach (var value in data)
//            //{
//            //    var character = CommonStrings.currentUser.data.userCharactersData.userCharacters[value.characterID].MakeCopy();

//            //    var characterView = minimalCharacterViewFactory.Create(CommonStrings.PrefabMinimalCharacterView);

//            //    //Component.DestroyImmediate(characterView.transform.GetComponent<DragableUIView>());

//            //    var cell = cellsViewArray[value.x, value.y];

//            //    characterView.SetCharacter(character, TeamType.Ally);
//            //    characterView.transform.SetParent(cell.transform);

//            //    characterView.transform.localScale = Vector3.one * .5f;

//            //    characterView.transform.localPosition = Vector3.zero;


//            //    if (cell.useOffsetCharView)
//            //        characterView.transform.localPosition -= Vector3.right * cell.HalfCellSize;

//            //    cell.allyCharacter = character;
//            //    cell.isBusy = true;

//            //    alliesOnField[value.y].Add(character);

//            //    // setup energy
//            //    var energy = characterView.GetComponent<CharacterEnergy>();
//            //    var skill = skillsManager.skills[character.skillId];

//            //    energy.SetData(skill, character, value.y);

//            //    characterView.GetComponent<CharacterViewGraphic>().UpdateColors(characterView.GetComponent<CharacterViewGraphic>().allyColor);
//            //}
//        }

//        public void AddEnemies(List<CharacterPosition> data)
//        {
//            //enemiesOnField = new List<Character>[6];
//            //for (int i = 0; i < enemiesOnField.Length; i++)
//            //    enemiesOnField[i] = new List<Character>();

//            //foreach (var value in data)
//            //{
//            //    try
//            //    {
//            //        var character = CharactersManager.DefaultCharacters.data.zombies[value.characterID].MakeCopy();
              
//            //        character.UpgradeAttributes(CommonStrings.matchSettings, GameFlow.CurrentMatchPreset.enemiesLevel - character.level);

//            //        //var characterView = detailCharacterViewFactory.Create(StringConstants.PrefabDetailCharacterView);
//            //        var characterView = minimalCharacterViewFactory.Create(CommonStrings.PrefabMinimalCharacterView);

//            //        var cell = cellsViewArray[value.x, value.y];

//            //        //Component.DestroyImmediate(characterView.transform.GetComponent<DragableUIView>());

//            //        characterView.SetCharacter(character, TeamType.Enemy);
//            //        characterView.transform.SetParent(cell.transform);

//            //        characterView.transform.localScale = Vector3.one * .5f;

//            //        characterView.transform.localPosition = Vector3.zero;

//            //        if (cell.useOffsetCharView)
//            //            characterView.transform.localPosition += Vector3.right * cell.HalfCellSize;

//            //        cell.enemyCharacter = character;
//            //        cell.isBusy = true;

//            //        enemiesOnField[value.y].Add(character);

//            //        characterView.GetComponent<CharacterViewGraphic>().UpdateColors(characterView.GetComponent<CharacterViewGraphic>().enemyColor);

//            //        // setup energy
//            //        var energy = characterView.GetComponent<CharacterEnergy>();
//            //        var skill = skillsManager.skills[character.skillId];

//            //        energy.SetData(skill, character, value.y);
//            //    }
//            //    catch
//            //    {
//            //        Debug.LogError(value.characterID);
//            //    }

//            //}
//        }

//        public void AddEnemies(List<CharacterPosition> data, Tower.Tower.TowerItem towerItem)
//        {
//        //    enemiesOnField = new List<Character>[6];
//        //    for (int i = 0; i < enemiesOnField.Length; i++)
//        //        enemiesOnField[i] = new List<Character>();

//        //    var budget = towerItem.residue;
//        //    int index = 0;
//        //    int[] budgetPerCharacter = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

//        //    while (budget > 0)
//        //    {
//        //        var i = UnityEngine.Random.Range(0, 7);
//        //        budgetPerCharacter[i] = budgetPerCharacter[i] + 1;
//        //        budget--;
//        //    }

//        //    foreach (var value in data)
//        //    {
//        //        try
//        //        {
//        //            var character = CharactersManager.DefaultCharacters.data.zombies[value.characterID].MakeCopy();
//        //            int targetLevel = towerItem.baseLevel + budgetPerCharacter[index];

//        //            character.UpgradeAttributes(CommonStrings.matchSettings, targetLevel - character.level);

//        //            index++;

//        //            //var characterView = detailCharacterViewFactory.Create(StringConstants.PrefabDetailCharacterView);
//        //            var characterView = minimalCharacterViewFactory.Create(CommonStrings.PrefabMinimalCharacterView);

//        //            var cell = cellsViewArray[value.x, value.y];

//        //            Component.DestroyImmediate(characterView.transform.GetComponent<DragableUIView>());

//        //            characterView.SetCharacter(character, TeamType.Enemy);
//        //            characterView.transform.SetParent(cell.transform);

//        //            characterView.transform.localScale = Vector3.one * .5f;

//        //            characterView.transform.localPosition = Vector3.zero;

//        //            if (cell.useOffsetCharView)
//        //                characterView.transform.localPosition += Vector3.right * cell.HalfCellSize;

//        //            cell.enemyCharacter = character;
//        //            cell.isBusy = true;

//        //            enemiesOnField[value.y].Add(character);

//        //            characterView.GetComponent<CharacterViewGraphic>().UpdateColors(characterView.GetComponent<CharacterViewGraphic>().enemyColor);

//        //            // setup energy
//        //            var energy = characterView.GetComponent<CharacterEnergy>();
//        //            var skill = skillsManager.skills[character.skillId];

//        //            energy.SetData(skill, character, value.y);
//        //        }
//        //        catch
//        //        {
//        //            Debug.LogError(value.characterID);
//        //        }

//        //    }
//        }

//        public List<Character> GetAllCharactersForSkillApply(Skill skill, Character character, int index)
//        {
//            var result = new List<Character>();
//            var targetTeam = (skill.targetTeam == TeamType.Ally) ? alliesOnField : enemiesOnField;

//            if (skill.place == SkillPlaceEnum.All)
//            {
//                foreach (var segment in targetTeam)
//                    result.AddRange(segment);

//                if (!skill.includeSelf && result.Contains(character))
//                    result.Remove(character);

//                return result;
//            }

//            if (skill.place == SkillPlaceEnum.Current)
//            {
//                result = targetTeam[index];

//                if (!skill.includeSelf && result.Contains(character))
//                    result.Remove(character);

//                return result;
//            }

//            if (skill.place == SkillPlaceEnum.Left)
//            {
//                if (index == 0)
//                    return result;

//                for (int i = index - 1; i >= 0; i--)
//                {
//                    result.AddRange(targetTeam[i]);
//                }

//                if (!skill.includeSelf && result.Contains(character))
//                    result.Remove(character);

//                if (skill.includeSelf && !result.Contains(character))
//                {
//                    result.Add(character);
//                }

//                return result;
//            }

//            if (skill.place == SkillPlaceEnum.Right)
//            {
//                if (index == 5)
//                    return result;

//                for (int i = index + 1; i <= 5; i++)
//                {
//                    result.AddRange(targetTeam[i]);
//                }

//                if (!skill.includeSelf && result.Contains(character))
//                    result.Remove(character);

//                if (skill.includeSelf && !result.Contains(character))
//                {
//                    result.Add(character);
//                }

//                return result;
//            }

//            return result;
//        }
//    }
//}
