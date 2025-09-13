//using Doozy.Engine;
//using Doozy.Engine.UI.Input;
//using Doozy.Engine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;
//using Zenject;
////using ZombieSoccer.Data;
//using ZombieSoccer.GameLayer.Characters;
////using ZombieSoccer.GameLayer.Commentator;
//using ZombieSoccer.GameLayer.Flow;
//using ZombieSoccer.GameLayer.Matching;
//using ZombieSoccer.GameLayer.Matching.Settings;
//using ZombieSoccer.GameLayer.Matching.States;
////using ZombieSoccer.GameLayer.Matching.Statistics;
//using ZombieSoccer.GameLayer.Matching.Time;
////using ZombieSoccer.GameLayer.States;
//using ZombieSoccer.GameLayer.Tower;
////using ZombieSoccer.GameLayer.Tower;
//using ZombieSoccer.GameLayer.UI;
//using System.Linq;
//using ZombieSoccer.GameLayer.AnimationSystem;
//using Sirenix.OdinInspector;
//using ZombieSoccer.UI.Widget;

//namespace ZombieSoccer.UI.Views
//{
//    public class ViewMatch : MonoBehaviour
//    {
//        //public MatchSlider matchSlider;
//        public GameObject statsButton;
//        public GameObject pausePlayButton;
//        //public GameObject resumeButton, restartButton, exitButton;
//        public GameObject pausePanel;
//        public GameObject statisticPanel;
//        public WidgetMatchTimer matchTimerView;
//        //public MatchResultView matchResultView;
//        public UIPopup myPopup;
//        public GameObject UserStatsPanel;
//        public static MatchStatsStructure CurrentMatchStats;

//        public MatchAnimationSystem matchAnimationSystem;
//        //[Inject]
//        //CharacterManager characterManager;

//        //[Inject]
//        //public MatchLogger matchLogger;

//        [Inject]
//        public Timer timer;

//        [Inject]
//        MatchStateMachine statMachine;

//        [Inject]
//        MatchScore matchScore;

//        [Inject]
//        MatchSettings matchSettings;

//        [Inject]
//        UIMatchField uiMatchField;

//        //[Inject]
//        //MatchStatistics matchStatistics;

//        [Inject]
//        Tower tower;

//        //[Inject]
//        //Commentator commentator;

//        public static event Action OnStartMatchEvent;
//        public static event Action OnCompleteMatchEvent;
//        public static event Action OnRestartMatchEvent;
//        public static event Action<int2> OnGoalEvent;

//        public static List<CharacterPosition> allies;
//        public static List<CharacterPosition> enemies;

//        public static int2 score = new int2(0, 0);
//        public bool IsPause { get; private set; }

//        public bool MatchInProgress { get; private set; }

//        public bool isTower { get; private set; }

//        public static bool completeSetup;

//        // private UIPopup m_popup;


//        public void SetMatchType(bool _isTower)
//        {
//            isTower = _isTower;
//            UserStatsPanel.SetActive(true);
//        }

//        protected void OnEnable()
//        {
//            if (!completeSetup)
//                return;

//            pausePanel.SetActive(false);
//            statisticPanel.SetActive(false);

//            score = new int2(0, 0);

//            timer.OnTimerCompleteEvent += CompleteMatch;
//            RestartMatch();

//            uiMatchField.AddAllies(allies);

//            if (!isTower)
//            {
//                //enemies = FieldPresetManager.EnemiesPositionsProcessing();
//                //uiMatchField.AddEnemies(enemies);
//            }
//            else
//            {
//                //enemies = FieldPresetManager.EnemiesPositionsProcessing(tower.towerItems[CommonStrings.currentUser.data.towerIndex]);
//                //uiMatchField.AddEnemies(enemies, tower.towerItems[CommonStrings.currentUser.data.towerIndex]);
//            }
//            CurrentMatchStats.EnemyCharacters = enemies;
//            uiMatchField.OnStartMatch();
//            StartMatch();

//            //var commentator = GameObject.FindObjectOfType<CommentatorView>();
//            //commentator.StartCommentator();
//            //commentator.gameObject.SetActive(false);

//        }



//        public void RestartMatch()
//        {
//            OnRestartMatchEvent?.Invoke();
//        }

//        public void StopMatch()
//        {
//            StopAllCoroutines();

//            timer.OnTimerCompleteEvent -= CompleteMatch;
//            MatchInProgress = false;
//            IsPause = false;
//            timer.ForceStop();

//            OnCompleteMatchEvent?.Invoke();
//        }

//        public void CompleteMatch()
//        {
//            if (!MatchInProgress)
//                return;

//            StopMatch();

//            Debug.Log("MatchManager : Complete Match");

//            //commentator.Pull(Commentator.CommentatorEventsEnum.CompleteMatchTime, string.Empty, string.Empty);

//            //matchLogger.Log();
//            //matchLogger.Log($"[RESULT MATCH SCOPE] Ally - {matchScore.MatchResult.x}; Enemy - {matchScore.MatchResult.y}");

//            score += matchScore.MatchResult;

//            //CommonStrings.currentUser.data.matchCount++;
//            //CommonStrings.currentUser.PushData();

//            if (score.x > score.y)
//            {
//                if (!isTower)
//                {
//                    GameFlow.OnAliiesWin();
//                }
//                else
//                {
//                    //CommonStrings.currentUser.data.towerIndex = CommonStrings.currentUser.data.towerIndex + 1;
//                    //CommonStrings.currentUser.PushData();
//                }
//            }

//            //GameObject.FindObjectOfType<CommentatorView>().StopCommentator(score);
//            Debug.LogError("---------------------------");
//            GameEventMessage.SendEvent("MatchEnded");
//            AddResultStructure();


//            //#endif

//        }

//        public void StartMatch()
//        {
//            GetComponentsInChildren<CharacterViewVisualFX>().ToList().ForEach(x => x.Setup());

//            Debug.Log("StartMatch");

//            OnStartMatchEvent?.Invoke();

//            MatchInProgress = true;
//            matchScore.Reset();
//            statMachine.Init();
//            //matchStatistics.ResetStatistics();

//            //matchLogger.Log(string.Empty, true);

//            //matchSlider.ResetHandle();
//            matchAnimationSystem.ResetSystem(true);
//            StartCoroutine(GameLoop());
//            CurrentMatchStats.PlayerCharacter = allies;

//        }

//        public static event Action OnNextStateEvent;

//        private bool matchIsAnimated = false;

//        private bool next;
//        public bool useAuto = true;

//        [Button]
//        void SetNext()
//        {
//            next = true;
//        }

//        IEnumerator GameLoop()
//        {
//            next = useAuto;

//            yield return new WaitForSeconds(1f);
//            matchIsAnimated = false;

//            while (MatchInProgress)
//            {
//                while (IsPause || !next)
//                {
//                    yield return new WaitForEndOfFrame();
//                }

//                if (!useAuto)
//                    next = false;

//                OnNextStateEvent?.Invoke();
//                statMachine.NextState?.Invoke();
//                matchIsAnimated = true;

//                matchAnimationSystem.PlayNext(() => matchIsAnimated = false);

//                while (matchIsAnimated)
//                    yield return new WaitForEndOfFrame();

//                yield return matchAnimationSystem.AnimateSkills();

//                Debug.LogError("------------------------------------------------------------");
//            }
//        }

//        public float timeMulty = 1f;

//        public void Update()
//        {
//            matchTimerView.UpdateData();
//            //matchResultView.UpdateData();
//        }

//        public void PauseButtonHandler()
//        {
//            IsPause = true;
//            timer.pause = IsPause;
//            myPopup = UIPopup.GetPopup("MenuPopup");
//            myPopup.GetComponent<WidgetMatchMenu>().ParentObj = transform;
//            myPopup.Show();
//            timer.matchPause = IsPause;
//            //pausePanel.SetActive(true);
//        }

//        public void ResumeButtonHandler()
//        {
//            IsPause = false;
//            timer.pause = IsPause;
//            myPopup.Hide();
//            timer.matchPause = IsPause;
//        }

//        public void RestartButtonHandler()
//        {
//            StopMatch();
//            GameEventMessage.SendEvent("RestartGameEvent");
//            myPopup.Hide();
//            //manager.SwapState(matchStatefactory.Create(allies, true));
//        }

//        public void ExitButtonHandler()
//        {
//            timer.ForceInvokeComplete();
//            StopMatch();
//            GameEventMessage.SendEvent("ExitGameEvent");
//            myPopup.Hide();

//            //manager.SwapState(mainMenufactory.Create());
//        }

//        void AddResultStructure()
//        {
//            if (score.x > score.y)
//            {
//                CurrentMatchStats.IsDefeat = false;
//            }
//            else
//            {
//                CurrentMatchStats.IsDefeat = true;
//            }

//            CurrentMatchStats.PlayerScore = score.x;
//            CurrentMatchStats.EnemyScore = score.y;
//        }
//    }
//}
