using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.Localization;

namespace ZombieSoccer.DebugMenu
{
    public class DebugLayer : DebugMenuPage
    {

        #region Injects

        [Inject]
        LocalizationManager localizationManager;

        #endregion

        #region UI Elements
        
        [SerializeField] Button BackButton;        

        [SerializeField] Button LogOpen;
        [SerializeField] Button WalletReset, WalletFill;
        [SerializeField] Button CharactersReset, CharactersUpgradeMaxLevel, CharactersUpgradeInitialLevel;
        [SerializeField] Button PiecesReset;
        [SerializeField] Button ScenarioReset;
        [SerializeField] Button LocaleRussian, LocaleEnglish;

        #endregion

        public const string Reset = "reset";
        public const string Fill = "fill";
        public const string UpgradeMaxLevel = "upgrade/maxlevel";
        public const string UpgradeInitialLevel = "upgrade/initialLevel";

        public override void Close()
        {
            base.Page.SetActive(false);
        }

        public override void Open()
        {
            base.Page.SetActive(true);
        }

        private void Awake()
        {
            WalletReset.onClick.AddListener(() => Wallet(Reset));
            WalletFill.onClick.AddListener(() => Wallet(Fill));

            CharactersReset.onClick.AddListener(() => Characters(Reset));
            CharactersUpgradeMaxLevel.onClick.AddListener(() => Characters(UpgradeMaxLevel));
            CharactersUpgradeInitialLevel.onClick.AddListener(() => Characters(UpgradeInitialLevel));

            PiecesReset.onClick.AddListener(() => Pieces(Reset));

            ScenarioReset.onClick.AddListener(() => Scenario(Reset));

            LocaleRussian.onClick.AddListener(() => localizationManager.SelectRussian());
            LocaleEnglish.onClick.AddListener(() => localizationManager.SelectEnglish());

        }        


        private async void Characters(string parameter)
        {
            string api = $"{Web.POST_ApiDebug}/characters/{parameter}";
            await Request($"{Web.UrlToServer}/{api}");
        }

        private async void Wallet(string parameter)
        {
            string api = $"{Web.POST_ApiDebug}/wallet/{parameter}";
            await Request($"{Web.UrlToServer}/{api}");
        }

        private async void Scenario(string parameter)
        {
            string api = $"{Web.POST_ApiDebug}/scenario/{parameter}";
            await Request($"{Web.UrlToServer}/{api}");
        }

        private async void Pieces(string parameter)
        {
            string api = $"{Web.POST_ApiDebug}/pieces/{parameter}";
            await Request($"{Web.UrlToServer}/{api}");
        }

        private async UniTask Request(string query)
        {
            var response = await Web.Request(Web.POST, query, Web.IdToken);
            Debug.Log(response);
        }
    }
}
