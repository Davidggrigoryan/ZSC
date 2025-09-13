using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZombieSoccer.GameLayer.Characters;
using System.Collections.Generic;
using Zenject;
using System.Linq;
using UniRx;
using ZombieSoccer.GameLayer.UI;
using ZombieSoccer.Models.MatchM;
using ZombieSoccer.UI.Presenters;
using ZombieSoccer.Models.DeckM;
using ZombieSoccer.UI.Widget;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Utitlies;
using System;
using System.Threading.Tasks;
using ZombieSoccer.Extensions;

namespace ZombieSoccer.UI.Views
{
    public class ViewMatchResult: BasePagePresenter
    {
        [Inject]
        private MatchResultCharacterView.Factory matchResultCharacterViewFactory;

        [Inject]
        private DeckModel deckModel;

        [Inject]
        private ResourcesManager resourcesManager;

        [Inject] private SignalBus _signalBus;

        private ReactiveProperty<MatchOutcome> reactiveMatchResult { get; set; } = new ReactiveProperty<MatchOutcome>();

        #region UI
        [SerializeField]
        private TMP_Text matchScoreText;

        [SerializeField]
        private WidgetShield enemyRandomShield;

        [SerializeField]
        private Transform allyViewsTeamRoot, enemyViewsTeamRoot;

        [SerializeField]
        private TMP_Text[] RewardsWallet;

        [SerializeField]
        private List<GameObject> resultWinElements, resultDefeatElements;

        [SerializeField]
        WidgetResources widgetCost;

        [SerializeField]
        Button continueButton;
        #endregion

        protected override void Inititalize()
        {
            base.Inititalize();

            reactiveMatchResult.Where(x => x != null && x.IsWin.HasValue).Subscribe(x =>
            {
                //matchScoreText.text = $"{x.PlayerTeamPowerScore}:{x.EnemyTeamPowerScore}";

                if (x.IsWin.Value)
                    resultWinElements.ForEach(x => x.SetActive(true));
                else
                    resultDefeatElements.ForEach(x => x.SetActive(true));

                RenderTeams(x.PlayerTeam, x.EnemyTeam);
            });

            continueButton.onClick.AddListener(() =>
            {
                MessageQueue.DefineDispatch(() =>
                {
                    _signalBus.Fire(new LocationWidgetSignal() { Show = true });
                    _signalBus.Fire(new PlanetWidgetSignal() { Show = false });
                    if (reactiveMatchResult.Value.IsWin.Value)
                        _signalBus.Fire(new MatchStateSignal() { IncrementMatchIndex = true });
                }, 1000);

                GetComponent<PageSignalHandler>().Fire("UIView_MainMenu");
            });
        }

        protected override void Enable()
        {
            base.Enable();

            allyViewsTeamRoot.DestroyChilds();
            enemyViewsTeamRoot.DestroyChilds();

            resultDefeatElements.ForEach(x => x.SetActive(false));
            resultWinElements.ForEach(x => x.SetActive(false));

            var pageSignalHandler = GetComponent<PageSignalHandler>();
            reactiveMatchResult.Value = (MatchOutcome)pageSignalHandler?.args?[0];
            //widgetCost.UpdateValues(new Wallet() { Crystal = 11, Dust = 11, Elixir = 11});
        }

        private void RenderTeams(TeamJson playerTeam, TeamJson enemyTeam)
        {
            var allyInstanceIds = playerTeam.Team.Select(c => c.Id).ToList();
            allyInstanceIds.ForEach(x =>
            {
                var arch = deckModel.Characters.data[x].archetypeId;
                matchResultCharacterViewFactory.Create(CommonStrings.PrefabMatchResultAllyCharacterView, CharactersManager.FindArchitypeById(arch), TeamType.Ally, 0, allyViewsTeamRoot);
            });

            CharactersManager.GetRandomCharactersList(TeamType.Enemy, 7)
                .ForEach(x => matchResultCharacterViewFactory.Create(CommonStrings.PrefabMatchResultEnemyCharacterView, x, TeamType.Ally, 0, enemyViewsTeamRoot));
        }
    }
}

