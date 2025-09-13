//using ZombieSoccer.GameLayer.Matching.Settings;

using System.Text;

namespace ZombieSoccer
{
    // Container class for constants, strings etc
    public static class CommonStrings
    {
        //public static MatchConstants matchConstants;

        // Paths to various database tables:
        public const string DBTableUserPath             = "DB_Users";
        public const string DBTableMatchSettingsPath    = "DB_MatchSettings";
        public const string DBTableDefaultCharactersPath= "DB_Characters";
        public const string DB_CitiesPath               = "DB_Scenario";

        public const string DBFieldUserInfoPath         = "Info";
        public const string DBFieldUserWalletPath       = "Wallet";
        public const string DBFieldUserCharactersPath   = "Characters";
        public const string DBFieldUserPortalDataPath   = "Portal";
        public const string DBFieldUserShieldDataPath   = "Shield";
        public const string DBFieldUserScenariosDataPath = "Scenarios";

        public const string PageManagerConfig           = "ScriptableObjects/PageManagerConfig";
        public const string PalleteTeamPositionsPath    = "ScriptableObjects/Pallete_TeamPositions";
        public const string PalleteColorsPath           = "ScriptableObjects/Pallete_Colors";
        public const string TextRarityColors            = "ScriptableObjects/TextRarityColors";
        public const string PalleteShieldPath           = "ScriptableObjects/Pallete_Shield";
        public const string CardBordersPath             = "ScriptableObjects/Card_Borders";        
        public const string CharacterAttributesPath     = "ScriptableObjects/CharacterAttributesScriptableObject";
        public const string MainButtonElementsPath      = "ScriptableObjects/MainButtonElementsScriptableObject";

        public const string PathAnimalsIcons            = "Icons/Animals";
        public const string PathElementsIcons           = "Icons/Elements";
        
        public const string PrefabCartForUpdate         = "AppUI/Elements/Views/CharacterUpdateRarity/CartForUpdate";
        public const string CartSlotForGrid             = "AppUI/Elements/Views/CharacterUpdateRarity/CartSlotForGrid";
        public const string UnlockSlotPrefab            = "AppUI/Elements/Views/CharacterUpdateRarity/ViewUnlockExample";
        public const string LockSlotPrefab              = "UI/Views/CharacterUpdateRarity/ViewLockExample";
        public const string SpinePrefab                 = "Characters/";
        public const string PrefabCharacterPositionPlaceholder = "AppUI/Elements/Views/CharacterPositionPlaceholder";

        public const string PrefabDetailCharacterView   = "AppUI/Elements/Views/DetCharacterView";
        public const string PrefabMatchResultAllyCharacterView = "AppUI/Elements/Views/VictoryDefeatPrefabs/MatchResultAllyCharactersView";
        public const string PrefabMatchResultEnemyCharacterView = "AppUI/Elements/Views/VictoryDefeatPrefabs/MatchResultEnemyCharactersView";

        public const string PrefabPortalSummonPopup     = "AppUI/PopUps/PortalSummonPopUp";
        public const string PrefabNotifyingPopup        = "AppUI/PopUps/NotifyingPopup";
        public const string StateSelectPopup            = "AppUI/PopUps/StateSelectPopup";
        public const string DisintegratePopup           = "AppUI/PopUps/DisintegratePopup";
        public const string ToShopMovePopup             = "AppUI/PopUps/ToShopMovePopup";
        public const string UpgradCharacterPopup        = "AppUI/PopUps/UpgradeCharacterPopup";

        //Detail character view popups
        public const string AboutPositionPopup          = "AppUI/PopUps/DetailCharacterView/AboutPositionPopup";
        public const string AboutElementsPopup          = "AppUI/PopUps/DetailCharacterView/AboutElementsPopup";
        public const string AboutRarityPopup            = "AppUI/PopUps/DetailCharacterView/AboutRarityPopup";

        public const string PrefabMinimalCharacterView  = "AppUI/Views/MinimalCharacterView";
        public const string PathCharactersSprites       = "Characters";

        public const string DeepLinkPrefab              = "Services/DeepLink/DeepLinkPrefab";
        public const string DebugMenuPrefab             = "Services/DebugMenu/DebugMenu";        
        public const string DeckPrefab                  = "Services/UserDeck/DeckPrefab";
        public const string TeamsGroupPrefab            = "Services/UserDeck/TeamsGroupPrefab";

        public const string MessagePopUp = "MessagePopUp";

        public const string StringMessageCollection = "MessageCollection";
        public const string StringUpdateRarityCollection = "CharacterUpdateRarityCollection";

        public const string PortalCollectionStrings = "PortalCollection";
        public const string PrefabPin = "Locations/PinPrefab";

        public const string ResourceUnitsPath           = "ResourceUnits";

        public static string KiloFormat(this int num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 1000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        public static string KiloFormatAccurate(this int num)
        {
            string numString = num.ToString();
            int length = numString.Length;
            string symbol = "K";
            if (length > 6) symbol = "M";
            if (length <= 3 || numString[numString.Length - 1] != '0')
                return numString;            
            StringBuilder sb = new StringBuilder();
            int k = length - (length / 3) * 3;
            var firstNumbers = k == 0 ? 3 : k;
            int i = 0;
            for (i = 0; i < firstNumbers; i++) sb.Append(numString[i]);
            sb.Append($",{numString.Remove(0, firstNumbers)}");
            i = sb.Length;
            while (sb[i-- - 1] == '0');
            sb.Remove(i + 1, sb.Length - i - 1);
            sb.Append(symbol);
            if (sb[sb.Length - 2] == ',') sb.Remove(sb.Length - 2, 1);
            return sb.ToString();
        }

    }
}
