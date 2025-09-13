using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.UI;
using ZombieSoccer.Utils.LocalStorage;
using System.Linq;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.UI.Presenters;
using ZombieSoccer.Localization;

namespace ZombieSoccer
{
    public class MergeConfirmPage : BasePagePresenter
    {

        #region Injects

        [Inject]
        PageManager pageManager;

        [Inject]
        CharacterAttributes rarityBackground;

        [Inject]
        LocalStorage localStorage;

        [Inject]
        LocalizationManager localizationManager;

        [Inject]
        CharacterAttributes rarityElements;

        [Inject]
        DetailCharacterView.Factory detailCharacterViewFactory;

        #endregion

        #region UI Elements

        [SerializeField]
        GameObject AttributesPanel;

        [SerializeField]
        GameObject ConfirmPage;

        [SerializeField]
        Image Background;

        [SerializeField]
        GameObject CartBefore;

        [SerializeField]
        GameObject CartAfter;

        [SerializeField]
        Image RarityBeforeImage;

        [SerializeField]
        Image RarityAfterImage;

        #endregion

        #region Fields

        (Character, Character) _charactersTuple;
        string _id1;
        string _id2;

        #endregion

        protected override void Inititalize()
        {
            base.Inititalize();

            ConfirmPage.GetComponent<Button>().onClick.RemoveAllListeners();
            ConfirmPage.GetComponent<Button>().onClick.AddListener(() =>
            {
                ConfirmFunc();
            });
        }
        
        protected override void Enable()
        {        
            base.Enable();

            _charactersTuple = ((Character, Character))GetComponentInChildren<PageSignalHandler>()?.args?[0];
            var selectedIds = (String[])GetComponentInChildren<PageSignalHandler>()?.args?[1];
            _id1 = selectedIds[1];
            _id2 = selectedIds[2];
            CorrectCarts(_charactersTuple.Item1, _charactersTuple.Item2);
            rarityBackground.UpdateBackgroundOnRarity(Background, (int)_charactersTuple.Item1.rarity);
            rarityElements.UpdateIconOnRarity(RarityBeforeImage, (int)_charactersTuple.Item1.rarity);
            rarityElements.UpdateIconOnRarity(RarityAfterImage, (int)_charactersTuple.Item2.rarity);
        }

        public void CorrectCarts(Character before, Character after)
        {
            var cartBefore = detailCharacterViewFactory.Create( before, TeamType.Ally, CartBefore.transform);
            var cartAfter = detailCharacterViewFactory.Create(after, TeamType.Ally, CartAfter.transform);
        }

        private async void ConfirmFunc()
        {
            string payload = JsonConvert.SerializeObject(new { ids = new string[2] { _id1, _id2 } });
            string mergeApi = string.Format(Web.POST_MergeApiFormat, _charactersTuple.Item1.instanceId);
            string api = $"{Web.UrlToServer}/{mergeApi}";

            var response = await Web.Request(Web.POST, api, Web.IdToken, payload);
            
            try
            {
                Dictionary<string, Character> dict = JsonConvert.DeserializeObject<Dictionary<string, Character>>(response);
                Character mergedCharacter = dict.Values.First();
                
                localStorage.AddNewCharacterIdx(mergedCharacter.instanceId);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                pageManager.FireMessage(MsgTypeEnum.Warning, await localizationManager.GetString(CommonStrings.StringUpdateRarityCollection, StringUpdateRarityCollectionEnum.MergeError.ToString()));
            }

            pageManager.Fire("UIView_PageOfMergeResult", (System.Object)_charactersTuple.Item2);
        }

    }
}
