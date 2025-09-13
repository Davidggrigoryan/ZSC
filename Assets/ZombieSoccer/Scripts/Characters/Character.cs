using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sirenix.OdinInspector;
using System;
using System.Text;
//using ZombieSoccer.GameLayer.Matching.Settings;
using ZombieSoccer.GameLayer.UI;
using UnityEngine;

namespace ZombieSoccer.GameLayer.Characters
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Serializable]
    public enum TeamType
    {
        Ally, Enemy
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [Serializable]
    public enum CharacterRole
    {
        None = 0,
        GK = 1, // голкипер        
        CB = 2, // or CD // центральный защитник        
        CM = 3, // центральный полузащитник        
        ST = 4 // форвард/нападающий
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [Serializable]
    public enum PassTypeEnum
    {
        Down,
        //Middle,
        Up
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [Serializable]
    public enum RarityEnum
    {
        Base = 1,
        Rare = 2,
        RarePlus = 3,
        Epic = 4,
        EpicPlus = 5,
        Legendary = 6,
        LegendaryPlus = 7,
        Genius= 8
    }

    /// <summary>
    /// пас и прием
    /// </summary>
    [Serializable]
    public struct PassAttributesStruct
    {
        public float downPassValue;
        public float upPassValue;
        //выбивание
        public float beatValue;

        public float GetPassValueByType(PassTypeEnum passType)
        {
            return (passType == PassTypeEnum.Down) ? downPassValue : upPassValue;
        }

        public float downReceiveValue;
        public float upReceiveValue;

        public float GetReceiveValueByType(PassTypeEnum passType)
        {
            return (passType == PassTypeEnum.Down) ? downReceiveValue : upReceiveValue;
        }
    }

    /// <summary>
    /// Конкуренция за мяч
    /// </summary>
    [Serializable]
    public struct CompetitionAttributesStruct
    {
        //Борьба
        public float fightValue;
        //Перехват
        public float interceptionValue;
        //Дриблинг
        public float dribbleValue;
        //Отбор
        public float pickingValue;
    }

    [Serializable]
    public struct MentalityAttributesStruct
    {
        //Оценка ситуации
        public float assessmentValue;
        //Стабильность
        public float stabilityValue;
    }

    [Serializable]
    public struct HitAttributeStruct
    {
        public float forceValue;
        //Точность
        public int accuracyValue;
        //Хитрость
        public int cunningValue;
    }

    [Serializable]
    public struct GoalkeeperAttributeStruct
    {
        //Обработка мяча
        public float ballProcessingValue;
        //Ловкость
        public int agilityValue;
        //Интуиция
        public int intuitionValue;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BuffType
    {
        Attack,
        Defence
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnimalsEnum
    {
        Woodpecker, Boar, Penguin, Beaver
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElementsEnum
    {
        Fire, Water, Earth, Air
    }

    [Serializable]
    public class CharcterInfo
    {
        public string activeSkillEn;
        public string passiveSkillEn;
        public string historyEn;
        public string historyRU;
    }

    [Serializable]
    public class Character
    {        

        [FoldoutGroup("General", expanded: true)]
        [JsonProperty("name")]
        public string characterName;

        /*[FoldoutGroup("General", expanded: true)]
        [ReadOnly]
        public string id;*/

        //public string? instanceId { get; set; }
        public string? archetypeId { get; set; }
        public string? portalName { get; set; }

        [FoldoutGroup("General", expanded: true)]
        [ReadOnly]
        public string instanceId;
        
        [FoldoutGroup("General", expanded: true)]
        public int level;

        public int maxLevel => ((int)rarity) * 10;

        public RarityEnum rarity;

        public AnimalsEnum animal;

        public ElementsEnum element;

        public CharacterRole role;

        public TeamType team;

        [JsonProperty]
        public float Dribb { get => dribb; set => dribb = UnityEngine.Mathf.Clamp(value, 0f, 1f); }
        public float dribb; // = .5f

        [FoldoutGroup("Character parameters", expanded: true)]
        public PassAttributesStruct passAttributes;

        [FoldoutGroup("Character parameters", expanded: true)]
        public CompetitionAttributesStruct competitionAttributes;

        [FoldoutGroup("Character parameters", expanded: true)]
        public MentalityAttributesStruct mentalityAttributes;

        [FoldoutGroup("Character parameters", expanded: true)]
        public HitAttributeStruct hitAttributes;

        [FoldoutGroup("Character parameters", expanded: true)]
        public GoalkeeperAttributeStruct goalkeeperAttributes;

        [FoldoutGroup("Character parameters", expanded: true)]
        public string skillId;

        public CharcterInfo charcterInfo;// = new CharcterInfo { };

        [JsonIgnoreAttribute]
        public ICharacterView characterView;

        public string GetCharcterNameWithPos()
        {
            return $"{characterName}";
        }
        public Character MakeCopy()
        {
            Character result = new Character();

            result.characterName = characterName;
            result.instanceId = instanceId;
            result.archetypeId = archetypeId;
            result.role = role;
            result.level = level;
            result.rarity = rarity;
            result.animal = animal;
            result.element = element;
            result.dribb = dribb;

            result.passAttributes = passAttributes;
            result.competitionAttributes = competitionAttributes;
            result.mentalityAttributes = mentalityAttributes;
            result.hitAttributes = hitAttributes;
            result.goalkeeperAttributes = goalkeeperAttributes;
            //result.skillId = skillId;
            //result.charcterInfo.activeSkillEn = charcterInfo.activeSkillEn;
            //result.charcterInfo.passiveSkillEn = charcterInfo.passiveSkillEn;
            //result.charcterInfo.historyEn = charcterInfo.historyEn;

            return result;
        }
        
        public bool RarityCanBeUpgraded()
        {
            bool archetypeCharacterExists = CharactersManager.DefaultCharacters.data.people.TryGetValue(this.archetypeId, out Character archetypeCharacter);
            if (!archetypeCharacterExists) return false;

            switch (archetypeCharacter.rarity)
            {
                case RarityEnum.Base:
                    return false;
                case RarityEnum.Rare:
                    if (this.rarity < RarityEnum.EpicPlus) // character with initial rarity of rare can't be upgraded beyond epic plus
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case RarityEnum.Epic:
                    return true;
                default:
                    return false;
            }
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Name: {characterName}; ");
            sb.Append($"Instance Id: {instanceId}; ");
            sb.Append($"Archetype Id: {archetypeId}; ");
            sb.Append($"Level: {level}; ");

            // PassAtutesStruct
            sb.Append($"downPass: {passAttributes.downPassValue}; ");
            sb.Append($"upPass: {passAttributes.upPassValue}; ");
            sb.Append($"downReceive: {passAttributes.downReceiveValue}; ");
            sb.Append($"upReceive: {passAttributes.upReceiveValue}; ");
            sb.Append($"beat: {passAttributes.beatValue}; ");

            //CompetiAttributesStruct
            sb.Append($"fight: {competitionAttributes.fightValue}; ");
            sb.Append($"interception: {competitionAttributes.interceptionValue}; ");
            sb.Append($"dribble: {competitionAttributes.dribbleValue}; ");
            sb.Append($"picking: {competitionAttributes.pickingValue}; ");

            //MentalitributesStruct
            sb.Append($"assessment: {mentalityAttributes.assessmentValue}; ");
            sb.Append($"stability: {mentalityAttributes.stabilityValue}; ");

            //HitAttreStruct
            sb.Append($"force: {hitAttributes.forceValue}; ");
            sb.Append($"accuracy: {hitAttributes.accuracyValue}; ");
            sb.Append($"cunning: {hitAttributes.cunningValue}; ");

            //GoalkeettributeStruct
            sb.Append($"ballProcessing: {goalkeeperAttributes.ballProcessingValue}; ");
            sb.Append($"agility: {goalkeeperAttributes.agilityValue}; ");
            sb.Append($"intuition: {goalkeeperAttributes.intuitionValue}; ");

            //SkillsAndHistoryStruct
            sb.Append($"ActiveSkillEn: {charcterInfo.activeSkillEn}; ");
            sb.Append($"PassiveSkillEn: {charcterInfo.passiveSkillEn}; ");
            sb.Append($"HistoryEn: {charcterInfo.historyEn}; ");

            return sb.ToString();
        }
    }
}