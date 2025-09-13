using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Characters;
using ZombieSoccer.UI.Presenters;
using ZombieSoccer.Utils.LocalStorage;

namespace ZombieSoccer.UI.ViewPortalM
{
    public class ViewPortal : BasePagePresenter
    {

        #region Injects

        [Inject]
        PageManager pageManager;

        [Inject]
        LocalStorage localStorage;

        [Inject]
        PortalSummonPopup.Factory portalSummonPopupFactory;

        [Inject]
        UserModel userModel;
        #endregion

        [SerializeField]
        GameObject randomCharacterPrefab;

        [SerializeField]
        GameObject darkMask;

        [SerializeField]
        Button summonButton;

        [SerializeField]
        TextMeshProUGUI timer;

        PortalSummonPopup portalSummonPopup = null;

        Wallet _summonCost;
        PortalTypeEnum _currentPortalType;
        int _totalSeconds;
        ReactiveProperty<bool> freeSummon = new ReactiveProperty<bool>(false);

        public enum PortalTypeEnum
        {
            People,
            Zombies
        }

        protected async override void Inititalize()
        {
            base.Inititalize();

            SetupSummonButton();

            portalSummonPopup ??= portalSummonPopupFactory.Create(CommonStrings.PrefabPortalSummonPopup);
            _currentPortalType = PortalTypeEnum.People;

            await GetPortalStatus();

            freeSummon.Where(e => true == e).Subscribe(_ =>
            {
                timer.text = "Free";
                //summonButton.onClick.AddListener(async () => {
                //    await GetPortalStatus();
                //    freeSummon.Value = false;
                //    SetupSummonButton(); 
                //});
            });
            SetupTimer();
        }

        private void SetupSummonButton()
        {
            summonButton.onClick.RemoveAllListeners();
            summonButton.onClick.AddListener(async () =>
            {
                CharacterWaitingAnimation(true);
                await Summon();
                CharacterWaitingAnimation(false);
            });
        }

        private async Task Summon()
        {
            if (!CheckForPossibility()) return;

            var idToken = Web.IdToken;
            string portalCalledApi = String.Format(Web.POST_PortalCalledApiFormat, _currentPortalType.ToString());
            string api = $"{Web.UrlToServer}/{portalCalledApi}";
            string response = await Web.Request(Web.POST, api, idToken, " ");
            if (freeSummon.Value)
            {
                freeSummon.Value = false;
                await GetPortalStatus();
            }
            Dictionary<string, Character> characterDictionary = JsonConvert.DeserializeObject<Dictionary<string, Character>>(response);
            if (characterDictionary != null)
            {
                var character = characterDictionary.Values.First();
                FirePortalPopup(character);
                localStorage.AddNewCharacterIdx(character.instanceId);
            }
            else
                Debug.LogError($"[ViewPortal] character is null, response: {response} ");
        }

        private bool CheckForPossibility()
        {
            if (true == freeSummon.Value) return true;
            if(userModel.Wallet.data.Crystal < _summonCost.Crystal)
            {
                pageManager.FireToShopMovePopup(Popups.ForActionNotEnoughEnum.Crystal);
                return false;
            }
            return true;
        }

        #region Timer procedures               

        private async UniTask GetPortalStatus()
        {
            var portalStatusStructure = new { price = new Wallet(), timeUntilFree = double.MaxValue };

            var idToken = Web.IdToken;
            string api = $"{Web.UrlToServer}/{"api/portal/People/status"}";
            var response = await Web.Request(Web.GET, api, idToken, " ");

            portalStatusStructure = JsonConvert.DeserializeAnonymousType(response, portalStatusStructure);

            _totalSeconds = (int)portalStatusStructure.timeUntilFree / 1000;
            _summonCost = portalStatusStructure.price;
        }

        private void SetupTimer()
        {
            Observable.Timer(
                System.TimeSpan.FromSeconds(1)).Repeat().Subscribe(_ =>
                {
                    if(_totalSeconds <= 0)
                    {
                        freeSummon.Value = true;
                        return;
                    }
                    _totalSeconds--;

                    var timeSpan = TimeSpan.FromSeconds(_totalSeconds);
                    int seconds = timeSpan.Seconds;
                    int minutes = timeSpan.Minutes;
                    int hours = timeSpan.Hours;
                    timer.text = $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
                });
        }

        #endregion

        private void FirePortalPopup(Character character)
        {
            pageManager.FirePortalPopup(character);
        }

        private void CharacterWaitingAnimation(bool active)
        {
            randomCharacterPrefab.SetActive(active);
            darkMask.SetActive(active);
        }
    }
}
