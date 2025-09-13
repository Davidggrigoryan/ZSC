using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
    

namespace ZombieSoccer
{
    public class SnapScrolling : MonoBehaviour
    {
        public Sprite[] ShieldSprites;
        
        private float panOffset = 0f;
        [Range(0f, 20f)]
        public float snapSpeed;
        [Range(0f, 50f)]
        public float scaleOffset;
        [Range(1f, 20f)]
        public float scaleSpeed;
        [Header("Other Objects")]
        public GameObject panPrefab;
        [SerializeField]
        private float[] namePositionsY = new float[9] { 7f, -11f, 35f, 38f, 9f, -6f, 7f, 15f, 9f };
        private float[] tapePositions = new float[9] { -28f, -24f, -45f, -52f, -34f, -14f, -26f, -38f, -38f };
        public GameObject[] instPans;

        private Vector2[] pansPos;
        private Vector2[] pansScale;

        private RectTransform contentRect;
        private Vector2 contentVector;

        private bool InScrolling;
        public int StartIndex { get; private set; }
        private int lastSpriteIndex;
        public int CurrentSpriteIndex;

        protected List<RectTransform> items = new List<RectTransform>();
        protected ScrollRect _scrollRect;
        private float _recordOffsetX = 0;
        private float _disableMarginX = 0;
        private float _threshold = 100f;

        public Vector2 _newAnchoredPosition;

        private Action ApplyChanging;

        public void SpawnPanels(int startIndex, Action callback)
        {
            this.ApplyChanging = callback;
            
            StartIndex = startIndex;
            InitParams();

            InitObject(0);
            instPans[0].transform.localPosition = new Vector2(panOffset, 0);
            pansPos[0] = -instPans[0].transform.localPosition;
            float addarg = 0f;
            //if (TargetObject.name.Equals("Details") || TargetObject.name.Equals("Tape"))
            //{
            //    addarg = 40f;
            //}
            float const0__ = panPrefab.GetComponent<RectTransform>().sizeDelta.x + addarg;

            for (int i = 1; i < ShieldSprites.Length; i++)
            {
                InitObject(i);

                float x = instPans[i - 1].transform.localPosition.x + const0__;
                float y = instPans[i - 1].transform.localPosition.y;
                instPans[i].transform.localPosition = new Vector2(x, y);

                pansPos[i] = -instPans[i].transform.localPosition;
            }
            
            GetPanelsForInfinite();
            MakeInfiniteScroll();
            
            contentRect.anchoredPosition = pansPos[StartIndex];
            //sizeDel2 = instPans[0].GetComponent<RectTransform>().sizeDelta.x;
            CurrentSpriteIndex = StartIndex;
            instPans[CurrentSpriteIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1);
            beginx = contentRect.anchoredPosition.x;
        }
        
        public void InitIndx(int indx)
        {            
            this.CurrentSpriteIndex = indx;
            contentRect.anchoredPosition = pansPos[CurrentSpriteIndex];
            instPans[CurrentSpriteIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1);
            beginx = contentRect.anchoredPosition.x;
        }

        private void InitParams()
        {
            contentRect = GetComponent<RectTransform>();
            instPans = new GameObject[ShieldSprites.Length];
            pansPos = new Vector2[ShieldSprites.Length];
            pansScale = new Vector2[ShieldSprites.Length];
        }

        private Vector3 vForScale = new Vector3(1f, 1f, 1f);
        private void InitObject(int index)
        {
            instPans[index] = Instantiate(panPrefab);
            instPans[index].transform.SetParent(transform);
            instPans[index].GetComponent<Image>().sprite = ShieldSprites[index];
            instPans[index].transform.localScale = vForScale;
        }

        void GetPanelsForInfinite()
        {
            foreach (GameObject panel in instPans)
            {
                items.Add(panel.GetComponent<RectTransform>());
            }

            //TODO FUCKING FUCK
            _recordOffsetX = items[2].GetComponent<RectTransform>().anchoredPosition.x - items[1].GetComponent<RectTransform>().anchoredPosition.x;
            _disableMarginX = _recordOffsetX * items.Count / 2;
            _scrollRect = gameObject.transform
                                .parent.gameObject.transform
                                    .parent.transform.gameObject.GetComponent<ScrollRect>();
        }
        private float maxground;
        void MakeInfiniteScroll()
        {
            for (int i = 0; i < items.Count; i++)
            {
                var itLocX = _scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).x; 
                if ( itLocX > _disableMarginX + _threshold )
                {
                    UpdateItemPosition( i, items.Count - 1, -1.0f );
                }
                else if ( itLocX < -_disableMarginX)
                {
                    UpdateItemPosition( i, 0, +1.0f );
                }
            }
        }

        
        private bool moved = false;        
        private float beginx = -1f;
        private float endx = -1f;
        private void UpdateItemPosition(int index, int childindex, float orientation )
        {
            _newAnchoredPosition = items[index].anchoredPosition;
            _newAnchoredPosition.x += items.Count * _recordOffsetX * orientation ;
            items[index].anchoredPosition = _newAnchoredPosition;
            if( orientation > 0 )
                _scrollRect.content.GetChild(0).transform.SetAsLastSibling();
            else
                _scrollRect.content.GetChild( childindex ).transform.SetAsFirstSibling();
        }

        private float distance;
        
        private void Update()
        {
            lastSpriteIndex = CurrentSpriteIndex;
            MakeInfiniteScroll();
            endx = beginx;
            beginx = contentRect.anchoredPosition.x;
            if (Mathf.Abs(Mathf.Abs(beginx) - Mathf.Abs(endx)) < 0.1f)
            {
                moved = false;
            }
            else moved = true;
            
            if (!InScrolling)
            {
                if (!moved)
                {
                    CorrectPosition();
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                distance = Mathf.Abs(Mathf.Abs(contentRect.anchoredPosition.x) - Mathf.Abs(items[i].transform.localPosition.x));
                CorrectScale(i);
                if (items[i].transform.localScale.x > 1.0f)
                {
                    CurrentSpriteIndex = i;
                }
            }
            
            if( CurrentSpriteIndex != lastSpriteIndex )
                ApplyChanging.Invoke();
        }
        
        private void CorrectScale( int index )
        {
            if( contentRect.anchoredPosition.x > 0 && items[index].anchoredPosition.x > 0
                ||
                contentRect.anchoredPosition.x < 0 && items[index].anchoredPosition.x < 0) return;
            
            float scale = Mathf.Clamp(1f / (distance / 15f)* 5f, 0.5f, 1.0f);
            float x = Mathf.SmoothStep(items[index].transform.localScale.x, scale+0.3f, 20f * Time.deltaTime);
            items[index].transform.localScale = new Vector3(x, x, 1.0f);
        }

        private void CorrectPosition()
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, -items[CurrentSpriteIndex].anchoredPosition.x, snapSpeed * Time.fixedDeltaTime);
            contentRect.anchoredPosition = contentVector ;
        }
        public void Scrolling(bool scroll)
        {
            InScrolling = scroll;
        }
    }
}