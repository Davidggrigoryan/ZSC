using InputObservable;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using ZombieSoccer.Extensions;

namespace ZombieSoccer.UI
{
    public class PlanetRotateOnTouch : RotateObjectOnTouch
    {
        public new PlanetRotateOnTouchWrapper BuildPipeline(InputObservableContext context)
        {
            return new PlanetRotateOnTouchWrapper(context);
        }

        public class PlanetRotateOnTouchWrapper : RotateObjectOnTouchWrapper
        {
            public PlanetRotateOnTouchWrapper(InputObservableContext context) : base(context)
            {
            }

            public PlanetRotateOnTouchWrapper SetTriggerToReturn()
            {
                Vector3 sourceAngles = new Vector3(0, 180, 0);
                float time = 4F;
                Vector3 rotationAnglesToReturn = Vector3.zero;
                Vector3 startObjectRotation = new Vector3(0F, 180F, 0F);
                bool beginToReturn = false;

                reactiveActions.Add(new Action(() =>
                {
                    if (onMove) { time = 4F; beginToReturn = false; }

                    time = Mathf.Clamp(time - Time.deltaTime, 0f, 4F);

                    if (Mathf.Approximately(time, 0F))
                    {
                        if (!beginToReturn) rotationAnglesToReturn = targetObject.transform.eulerAngles;
                        beginToReturn = true;
                        rotationAnglesToReturn = Vector2.Lerp(rotationAnglesToReturn, startObjectRotation, 0.01F);
                        targetObject.transform.eulerAngles = rotationAnglesToReturn;
                    }
                }));
                return this;
            }

            public PlanetRotateOnTouchWrapper SetScaleTrigger()
            {
                var distance = 4F;
                var touch1 = context.GetObservable(1);
                bool onMoveTouch1 = false;

                reactiveActions.Add(new Action(() =>
                {
                    Action<float> changeDistance = (targetDistance) =>
                    {
                        var currentDist = Vector3.Distance(camera.transform.localPosition, targetObject.transform.localPosition);
                        var lerpDist = Mathf.Lerp(currentDist, targetDistance, 0.01F);
                        var dir = new Vector3(0, 0, -lerpDist);
                        camera.transform.localPosition = dir;
                    };

                    if (onMove && onMoveTouch1)
                        changeDistance(distance);
                    else
                        changeDistance(4F);
                }));

                touch1.OnBegin.Subscribe(_ =>
                {
                    RectangleObservable.From(touch, touch1)
                    .PinchSequence()
                    .TakeWhile((v) => onMove && onMoveTouch1)
                    .Subscribe(diff =>
                    {
                        float updatedDistance = 0F;                        

                        if (diff.x > 0 && diff.y > 0)
                            updatedDistance -= 0.1F;
                        else if (diff.x <= 0 && diff.y <= 0)
                            updatedDistance += 0.1F;
                        if (updatedDistance > 4F || updatedDistance < 0)
                            updatedDistance = 4F;

                        distance = updatedDistance;
                    });
                });

                return this;
            }
        }
    }
}
