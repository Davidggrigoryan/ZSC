using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.ApplicationLayer.Data;
using ZombieSoccer.Models;
using ZombieSoccer.UI.Widget;
//using ZombieSoccer.Data;
using ZombieSoccer.Utitlies;
using ZombieSoccer.ZombieSoccer.Scripts.Locations.Pin;
using ZombieSoccer.ZombieSoccer.Scripts.MessageHandlers.GameFlowNS;
using ZombieSoccer.ZombieSoccer.Scripts.Models.Messages.MapsWidget;

namespace ZombieSoccer.GameLayer.Flow
{
    public class GameFlow : WidgetBase
    {
        public MatchPreset[] matchPresets;

        private GameLocation[] locations;

        public GameLocation currentGameLocation { get; private set; } // TODO: change to gamelocation

        public MatchPreset currentMatchPreset { get; private set; }

        public Sprite currentPin, visitedPin, lockedPin;

        public float pinCurrentScale = 3f, pinOtherScale = 1f;

        [Inject] 
        ScenarioModel scenarioModel;

        [Inject]
        UserModel userModel;

        [Inject] 
        GameFlowMessageHandler _gameFlowMessageHandler;

        [Inject]
        SignalBus _signalBus;
        
        [Inject]
        Pin.Pool _pinsPool;
        
        [SerializeField] private string cityName = "-MtCV8-l7lZ7rkNOJTot";
        [SerializeField] private UILineRenderer _uiLineRenderer;
        private string currentMatchId;
        private City currentCity;
        private bool instanced = false;
        private int currentLocation = 0;

        /// <summary>
        /// Inits data, sends message to MapsWidget that init is completed
        /// </summary>
        protected override void Inititalize()
        {
            currentCity = scenarioModel.Cities.data[cityName];
            matchPresets = currentCity.matches.Select(m => new MatchPreset(m.id, m.powerScore, m.order)).ToArray();
            _gameFlowMessageHandler.SetGameFlow(this);
            _signalBus.Fire<MapsWidgetMessage>(new InitMapsWidgetLocationsMessage());
        }

        public override void Enable()
        {
            base.Enable();
            CalculatePins();
        }

        public void CalculatePins()
        {
            currentMatchId = userModel.Scenarios.data[cityName];
            if (currentMatchPreset == null || currentMatchId != currentMatchPreset.matchId )
            {
                currentMatchPreset = Array.Find(matchPresets, m => m.matchId == currentMatchId);
            }
            
            if (!instanced)
            {
                InstanceWithPins(); // initial 
                instanced = true;
            }

            if ((currentMatchPreset.matchIndex >= currentGameLocation.maxPins 
                && (currentMatchPreset.matchIndex / currentGameLocation.maxPins) != currentLocation)
                || currentMatchPreset.matchIndex < currentGameLocation.maxPins && currentLocation != 0) // change location
            {
                InstanceWithPins();
            }
            
            currentGameLocation.UpdatePinsStates(currentMatchPreset.matchIndex % currentGameLocation.maxPins);
        }

        private void InstanceWithPins(int maxPins = 35)
        {
            currentLocation = currentMatchPreset.matchIndex /  maxPins;
            if (currentGameLocation == null)
            {
                InstanceLocation(currentLocation);
            }
            else
            {
                SetLocationData(currentLocation, currentGameLocation);
            }

            if (_uiLineRenderer.bezierPath == null)
                _uiLineRenderer.bezierPath = new BezierPath();

            _uiLineRenderer.bezierPath.SetControlPoints(currentGameLocation.GetPoints());
            _uiLineRenderer.Points = currentGameLocation.GetPoints();
            currentGameLocation.AddPin(currentGameLocation.maxPins, _uiLineRenderer);
        }

        private void SetLocationData(int location, GameLocation scriptable)
        {
            LocationData locationData = currentCity.locations.Find(l => l.order == location);
            Vector2[] points = locationData.points.Select(point => new Vector2(point.x, point.y)).ToArray();
            
            scriptable.SetPoints(points);
            scriptable.SetTexture(locationData.imagePath);
        }
        
        private void InstanceLocation(int location)
        {
            GameLocation scriptable = ScriptableObject.CreateInstance<GameLocation>();
            //scriptable.SetLocationWidget(this);
            //scriptable.SetGameFlow(this);
            scriptable.SetPinsAnimation(GetComponentInParent<PinsAnimation>());
            scriptable.PositionRelativeToGameFlow();
            scriptable.SetPinsPool(_pinsPool);
            SetLocationData(location, scriptable);
            
            currentGameLocation = scriptable;
        }
        
        public void OnAlliesWin()
        {
            Debug.Log("Allies win!");
        }
        
        /// <summary>
        /// This is for testing purposes
        /// </summary>
        [Button]
        public void AddLocationToRoot()
        {
            GameLocation scriptable = ScriptableObject.CreateInstance<GameLocation>();
            //scriptable.SetGameFlow(this);
            scriptable.SetPinsAnimation(GetComponentInParent<PinsAnimation>());
            scriptable.PositionRelativeToGameFlow();
            SetLocationData(2, scriptable);
            
            scriptable.Hide();
            
            _signalBus.Fire<MapsWidgetMessage>(new AddLocationToMapsWidgetMessage(scriptable));
        }
        
        [Button]
        public void RecalculatePins()
        { 
            SetLocationData(currentLocation, currentGameLocation);
            
            currentGameLocation.AddPin(currentGameLocation.maxPins, _uiLineRenderer);
            currentGameLocation.UpdatePinsStates(currentMatchPreset.matchIndex % currentGameLocation.maxPins);
        }
        
        [Button]
        public void UpdateData()
        {
            locations = GetComponentsInChildren<GameLocation>();
            int offset = 0;

            foreach (var location in locations)
            {
                location.offset = offset;
                offset += location.pins.Count;
            }
        }

        [Button]
        private void Setup()
        {
            var ls = GetComponentsInChildren<GameLocation>();
            for (int i = 0; i < 10; i++)
            {
                ls[i].AddPin(35, _uiLineRenderer);
            }

            UpdateData();
        }
    }
}
