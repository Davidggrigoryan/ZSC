using DG.Tweening;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.GameLayer.Flow;
using ZombieSoccer.Models;
using ZombieSoccer.UI.Widget;
using ZombieSoccer.ZombieSoccer.Scripts.Locations.Pin;

namespace ZombieSoccer.UI
{

    public sealed class LocationWidgetSignal
    {
        public bool Show { get; set; }
        public Action action { get; set; }
    }

    public sealed class MatchStateSignal
    {
        public bool IncrementMatchIndex { get; set; }
    }

    [DisallowMultipleComponent]
    public sealed class LocationWidget : WidgetBase
    {
        [Inject] private UserModel _userModel;
        [Inject] private SignalBus _signalBus;
        [Inject] private ScenarioModel _scenarioModel;
        [Inject] private Pin.Pool _pinsPool;

        [SerializeField] private UILineRenderer _uiLineRenderer;
        [SerializeField] private GameObject background2DMap;
        [SerializeField] private GameObject scaleTargetMap2D;

        public Sprite currentPin, visitedPin, lockedPin;
        public float pinCurrentScale = 3f, pinOtherScale = 1f;
        private ReactiveProperty<int> _cityIndex = new ReactiveProperty<int>();
        private ReactiveProperty<int> _matchIndex = new ReactiveProperty<int>();        
        
        GameLocation gameLocation;

        public GameObject Root => base.root;

        protected override void Inititalize()
        {
            base.Inititalize();
            _signalBus.Subscribe<LocationWidgetSignal>(LocationWIdgetSignalReceiver);
            _signalBus.Subscribe<MatchStateSignal>((MatchStateSignal message) => _matchIndex.Value += message.IncrementMatchIndex ? 1 : 0);
            
            if (_uiLineRenderer.bezierPath == null)
                _uiLineRenderer.bezierPath = new BezierPath();
            
            gameLocation = InstanceLocation();

            var cityName = "-MtCV8-l7lZ7rkNOJTot";
            
            _cityIndex.Subscribe(cityIndex =>
            {
                var currentCity = _scenarioModel.Cities.data[cityName];
                var matchPresets = currentCity.matches.Select(m => new MatchPreset(m.id, m.powerScore, m.order)).ToArray();
                //var currentMatchPreset = Array.Find(matchPresets, m => m.matchId == _userModel.Scenarios.data[cityName]);
                //_matchIndex.Value = currentMatchPreset.matchIndex;
                
                //LocationData locationData = currentCity.locations.Find(l => l.order == _cityIndex.Value);
                //Vector2[] points = locationData.points.Select(point => new Vector2(point.x, point.y)).ToArray();

                //gameLocation.SetPoints(points);
                //gameLocation.SetTexture(locationData.imagePath);

                //_uiLineRenderer.bezierPath.SetControlPoints(gameLocation.GetPoints());
                //_uiLineRenderer.Points = gameLocation.GetPoints();
                //gameLocation.AddPin(gameLocation.maxPins, _uiLineRenderer);
            });

            _matchIndex.Subscribe(matchIndex =>
            {
                gameLocation.UpdatePinsStates(matchIndex);
            });
            
            _cityIndex.Value = 0;
        }

        private void LocationWIdgetSignalReceiver(LocationWidgetSignal message)
        {
            switch (message.Show)
            {
                case true:
                    {
                        this.gameLocation.Show();
                        root.GetComponent<CanvasGroup>().DOFade(1f, 1f);
                        background2DMap.GetComponent<CanvasGroup>().DOFade(1f, 1f);
                        scaleTargetMap2D.transform.localScale = Vector3.zero;
                        scaleTargetMap2D.transform.DOScale(1f, 1f);
                        break;
                    }                    
                case false:
                    {
                        this.gameLocation.Hide();
                        root.GetComponent<CanvasGroup>().DOFade(0F, 1F);
                        background2DMap.GetComponent<CanvasGroup>().DOFade(0f, 1f);
                        scaleTargetMap2D.transform.DOScale(0f, 1f);
                    }
                    break;
            }
        }

        private GameLocation InstanceLocation()
        {
            GameLocation scriptable = ScriptableObject.CreateInstance<GameLocation>();
            scriptable.SetLocationWidget(this);
            scriptable.PositionRelativeToGameFlow();
            scriptable.SetPinsAnimation(GetComponentInChildren<PinsAnimation>());
            scriptable.SetPinsPool(_pinsPool);
            return scriptable;
        }
    }

    
}


