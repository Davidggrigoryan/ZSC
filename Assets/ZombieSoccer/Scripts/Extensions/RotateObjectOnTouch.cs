using InputObservable;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ZombieSoccer.Extensions
{
    public class RotateObjectOnTouch
    {

        public RotateObjectOnTouchWrapper BuildPipeline(InputObservableContext context)
        {
            return new RotateObjectOnTouchWrapper(context);
        }

        public class RotateObjectOnTouchWrapper
        {
            protected InputObservableContext context;
            protected GameObject targetObject = new GameObject();
            protected IInputObservable touch;
            protected CompositeDisposable compositeDisposoble;
            protected Camera camera;

            protected Vector3 rotationAngles = Vector3.zero;
            protected bool onMove = false;

            protected List<Action> reactiveActions = new List<Action>();

            public RotateObjectOnTouchWrapper(InputObservableContext context)
            {
                this.context = context;
                this.touch = context.GetObservable(0);
            }

            public RotateObjectOnTouchWrapper SetTargetObject(GameObject targetObject)
            {
                this.targetObject = targetObject;
                return this;
            }

            public RotateObjectOnTouchWrapper SetCamera(Camera camera)
            {
                this.camera = camera;
                return this;
            }

            public RotateObjectOnTouchWrapper SetCompositeDisposable(CompositeDisposable compositeDisposable)
            {
                this.compositeDisposoble = compositeDisposable;
                return this;
            }

            public RotateObjectOnTouchWrapper Subscribe()
            {
                var sp = targetObject.transform.eulerAngles;
                rotationAngles = Vector3.zero;
                var startCameraRotation = new Vector2(0, 180);
                targetObject.transform.eulerAngles = startCameraRotation;

                reactiveActions.Add(new Action(() =>
                {
                    onMove = false;
                    rotationAngles = Vector3.Lerp(rotationAngles, Vector3.zero, 0.01F);
                    targetObject.transform.eulerAngles += rotationAngles;
                }));

                touch.Difference()
                    .Subscribe(v2 =>
                    {
                        var rot = v2.ToEulerAngle(3F, 3F);
                        rotationAngles.x = Mathf.Clamp(rot.x * 0.005F, -3F, 3F);
                        rotationAngles.y = -Mathf.Clamp(rot.y * 0.005F, -3F, 3F);
                        onMove = true;
                    }).AddTo(compositeDisposoble);

                Observable.EveryLateUpdate().Subscribe(_ =>
                {
                    reactiveActions.ForEach(action => action());
                }).AddTo(compositeDisposoble);
                return this;
            }
        }
    }
}
