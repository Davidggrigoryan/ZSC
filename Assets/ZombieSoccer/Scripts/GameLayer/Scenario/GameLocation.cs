using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using ZombieSoccer.UI;
using ZombieSoccer.ZombieSoccer.Scripts.Locations.Pin;

namespace ZombieSoccer.GameLayer.Flow
{
    public class GameLocation :  IGameLocation
    {
        public List<Pin> pins = new List<Pin>();
        public int offset;
        public int maxPins = 35; // May be set for each individually
        
        private LocationWidget _gameFlow;
        private RectTransform _rectTransform;
        private PinsAnimation _pinsAnimation;

        private Image _image;
        private Vector2[] _points;

        private Pin.Pool _pinsPool;
        
        [SerializeField]
        private UILineRenderer uiLineRenderer;

        public void OnEnable()
        {
            _gameObject = new GameObject("location", typeof(RectTransform));
            _image = _gameObject.AddComponent<Image>();
            _rectTransform = _gameObject.GetComponent<RectTransform>();
            
            _gameObject.SetActive(true);
        }

        public void PositionRelativeToGameFlow()
        {
            _gameObject.transform.SetParent(_gameFlow.Root.transform);
            
            _rectTransform.offsetMin = new Vector2(0, _rectTransform.offsetMin.y);
            _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x, 0);
                
            _rectTransform.offsetMax = new Vector2(0, _rectTransform.offsetMax.y);
            _rectTransform.offsetMax = new Vector2(_rectTransform.offsetMax.x, 0);
            
            _gameObject.transform.localScale = new Vector3(1, 1, 1);
            _gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        public void SetTexture(string path)
        {
            var texture = Resources.Load<Texture2D>(path);
            Debug.LogError(path);
            _image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), _rectTransform.pivot);
            _image.SetNativeSize();
        }
        
        public void UpdatePinsStates(int currentMatchIndex)
        {
            for (int i = 0; i < pins.Count; i++)
            {
                var img = pins[i].GetComponent<Image>();
                pins[i].transform.localScale = Vector3.one * _gameFlow.pinOtherScale;

                if (i + offset < currentMatchIndex)
                {
                    img.sprite = _gameFlow.visitedPin;
                    img.enabled = true;
                }

                if (i + offset == currentMatchIndex)
                {
                    img.sprite = _gameFlow.currentPin;
                    //pins[i].localScale = Vector3.one * gameFlow.pinCurrentScale;
                    img.transform.SetAsLastSibling();
                    _pinsAnimation.AnimateCurrentMatchPin(img.transform);
                }

                if (i + offset > currentMatchIndex)
                {
                    img.sprite = _gameFlow.lockedPin;
                }
            }
        }
        
        public void AddPin(int count, UILineRenderer uilr)
        {
            
            if (count > pins.Count)
            {
                int amountToSpawn = count - pins.Count;
                for (int i = 0; i < amountToSpawn; i++)
                {
                    pins.Add(_pinsPool.Spawn());
                }
            } 
            else if (count < pins.Count)
            {
                int amountToDespawn = pins.Count - count;
                List<Pin> pinsToDespawn = pins.Take(amountToDespawn).ToList();
                pins.RemoveRange(0, amountToDespawn);
                
                for (int i = 0; i < amountToDespawn; i++)
                {
                    _pinsPool.Despawn(pinsToDespawn[i]);
                }
            }
            
            var line = uilr.bezierPath.GetDrawingPoints0();
            var step = line.Count / count;

            for (int i = 0; i < count; i++)
            {
                var pin = pins[i];
                pin.transform.parent = _gameObject.transform;
                pin.GetComponent<Image>().sprite = _gameFlow.lockedPin;

                pin.transform.localScale = Vector3.one * _gameFlow.pinOtherScale;

                pin.transform.localPosition = (line[step * i] - Vector2.one * 0.5f) * _rectTransform.rect.size;

                Debug.Log(line[step * i]);
            }
           
        }

        public override void Hide()
        {
            base.Hide();
            if (_pinsAnimation != null) // for example if it's a 3d model
            {
                _pinsAnimation.timeline.Stop();
            }
        }
        
        public override void Show()
        {
            base.Show();
            if (_pinsAnimation != null) // for example if it's a 3d model
            {
                _pinsAnimation.timeline.Play();
            }
        }

        public void SetPinsPool(Pin.Pool pinsPool)
        {
            _pinsPool = pinsPool;
        }
        
        public Vector2[] GetPoints()
        {
            return _points;
        }

        public void SetPoints(Vector2[] points)
        {
            _points = points;
        }
        
        public void SetLocationWidget(LocationWidget gameFlow)
        {
            _gameFlow = gameFlow;
        }      
        
        public void SetPinsAnimation(PinsAnimation pinsAnimation)
        {
            _pinsAnimation = pinsAnimation;
        }
        
        public void SetRectTransform(RectTransform rectTransform)
        {
            _rectTransform = rectTransform;
        }

        public override void SetGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public override GameObject GetGameObject()
        {
            return _gameObject;
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            for (int i = 0; i < pins.Count; i++)
            {
                UnityEditor.Handles.Label(pins[i].transform.position, $"{i + offset}");
            }
        }
#endif
    }
    
}