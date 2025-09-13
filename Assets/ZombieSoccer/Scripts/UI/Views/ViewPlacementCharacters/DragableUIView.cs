using InputObservable;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using ZombieSoccer.Models.TeamM;
using ZombieSoccer.ReactiveInput;

namespace ZombieSoccer.GameLayer.UI
{
    public class DragableUIVIewSignal
    {

    }

    public class DragableUIView : EventTrigger, ReactiveDraggable
    {
        [ReadOnly]
        public CharacterPositionPlaceholder currentDragAndDropTarget;
        
        private Transform previousTransform;
        public bool canDrag = true;

        private float _screenHalfByX;
        private float _screenHalfByY;
        private Vector3 _screenHalfVector;

        // drag
        private ObservableInputContextService inputContextService;
        private IInputObservable io;

        private bool unsubscribed;
        private bool toDestroyCurrentGameObject;
        private bool dragging;

        TeamsGroupModel teamsGroupModel;
        SignalBus signalBus;

        private void Start()
        {
            _screenHalfByX = Screen.currentResolution.width/2f;
            _screenHalfByY = Screen.currentResolution.height / 2f;
#if UNITY_EDITOR
            _screenHalfByX = Screen.currentResolution.height / 2f;
            _screenHalfByY = Screen.currentResolution.width/2f;
#endif
            _screenHalfVector = new Vector3(_screenHalfByX, _screenHalfByY, 0f);
            
            var characterPositionPlaceholder = GetComponentInParent<CharacterPositionPlaceholder>();
            teamsGroupModel = characterPositionPlaceholder.teamsGroupModel.Value;
            signalBus = characterPositionPlaceholder.signalBus;

            InitDraggableVariables();
            CreateInputContext();
        }

        public void CreateInputContext()
        {
#if UNITY_EDITOR || UNITY_WEBGL
            io = new MouseInputContext(this, null).GetObservable(0);
#elif UNITY_ANDROID || UNITY_IOS
            io = new TouchInputContext(this, null).GetObservable(0);
#endif
        }

        public void InitDraggableVariables()
        {
            toDestroyCurrentGameObject = false;
            unsubscribed = true;
            dragging = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("On pointer down, :" + gameObject.name);

            toDestroyCurrentGameObject = false;
            dragging = true;

            if (unsubscribed)
            {
                unsubscribed = false;
                Func<InputEvent, bool> takeWhile = (e) => TakeWhile(e);
                Action<InputEvent> onBegin = e => Begin(e);
                Action<InputEvent> onDrag = e => Dragging(e);
                Action<InputEvent> onDragEnd = e => DragEnd(e);
                Action onComplete = () => OnComplete();
                Action onUnsubscribe = () => OnUnsubscribe();

                ObservableInputObjectWrapper onBeginWrapper = ObservableInputObjectBuilder.Create().AddOnAction(onBegin);
                ObservableInputObjectWrapper onMoveWrapper = ObservableInputObjectBuilder.Create().AddOnAction(onDrag);
                ObservableInputObjectWrapper onEndWrapper = ObservableInputObjectBuilder.Create().AddOnAction(onDragEnd);

                inputContextService.DragAndDrop(io, onBegin, onDrag, onDragEnd, onComplete, onUnsubscribe, this, takeWhile);
            }
        }


        public override void OnPointerUp(PointerEventData obj)
        {
            Debug.LogError($"OnPointerUp: {gameObject.name}");            
        }

        public void Begin(InputEvent e)
        {
            if (!canDrag)
                return;

            previousTransform = transform.parent;
            transform.SetParent(GetComponentInParent<Canvas>().transform);
        }

        public bool TakeWhile(InputEvent e)
        {
            return dragging;
        }

        public void OnComplete()
        {
            if (toDestroyCurrentGameObject)
            {
                try
                {
                    gameObject.GetComponent<DetailCharacterView>().Dispose();
                }
                catch (NullReferenceException e) { }
            }
        }

        public void OnUnsubscribe()
        {
            unsubscribed = true;
        }

        public void Dragging(InputObservable.InputEvent e)
        {
            if (dragging)
            {
                var pos = new Vector3(e.position.x, e.position.y, 0f);
                pos.x += _screenHalfByX / 2;
                transform.localPosition = pos - _screenHalfVector;
            }
        }

        public void DragEnd(InputObservable.InputEvent e)
        {
            if (!canDrag)
                return;

            if (this == null)
                return;

            dragging = false;
            CharacterPositionPlaceholder raycastResult = Raycast(e);
            if (raycastResult != null)
            {
                Debug.LogError($"Raycast result: {raycastResult.IsBusy}");

                var current = currentDragAndDropTarget.GetObjectFromTarget();
                if (null == currentDragAndDropTarget) return;
                if (raycastResult.IsBusy)
                {
                    Debug.LogError("Setting character to view");
                    var target = raycastResult.GetObjectFromTarget();

                    currentDragAndDropTarget.SetObjectToTarget(target);
                    target.gameObject.GetComponent<DragableUIView>().UpdateCurrentDragAndDropTarget();

                    teamsGroupModel.ChangePositions(currentDragAndDropTarget.Index, raycastResult.Index);
                }
                else
                {
                    currentDragAndDropTarget.ResetTarget();
                    teamsGroupModel.RemoveCharacter(currentDragAndDropTarget.Index);
                    teamsGroupModel.AddCharacter(raycastResult.Index, currentDragAndDropTarget.GetObjectFromTarget().character.instanceId);
                }

                raycastResult.SetObjectToTarget(current);
                current.gameObject.GetComponent<DragableUIView>().UpdateCurrentDragAndDropTarget();
            }
            else
            {
                currentDragAndDropTarget.ResetTarget();
                teamsGroupModel.RemoveCharacter(currentDragAndDropTarget.Index);
                toDestroyCurrentGameObject = true;
            }
            signalBus.Fire(new DragableUIVIewSignal() { });
        }

        private CharacterPositionPlaceholder Raycast(InputObservable.InputEvent e)
        {
            PointerEventData m_PointerEventData;

            var m_Raycaster = GetComponentInParent<GraphicRaycaster>();
            //Fetch the Event System from the Scene
            var m_EventSystem = GetComponentInParent<EventSystem>();

            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = e.position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                var target = result.gameObject.GetComponent<CharacterPositionPlaceholder>();

                Debug.Log(
                    $"Hit: {result.gameObject.name}, is DragAndDropTarget: {target != null}, target busy: {target?.IsBusy}");

                if (target != null) // && !target.IsBusy)
                {
                    return target;
                }
            }

            return null;
        }

        public void UpdateCurrentDragAndDropTarget()
        {
            currentDragAndDropTarget = GetComponentInParent<CharacterPositionPlaceholder>();
            if (!currentDragAndDropTarget.IsBusy)
                currentDragAndDropTarget.ResetTarget();
        }

        public void SetInputContextService(ObservableInputContextService inputContextService)
        {
            this.inputContextService = inputContextService;
        }
    }
}