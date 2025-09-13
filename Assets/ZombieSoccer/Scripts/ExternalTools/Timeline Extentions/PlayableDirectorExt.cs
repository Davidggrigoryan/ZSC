using System;
using UnityEngine.Playables;
using UniRx;

public static class PlayableDirectorExt
{
    public static IObservable<PlayableDirector> PlayedAsObservable(this PlayableDirector src)
    {
        return Observable.FromEvent<PlayableDirector>(h => src.played += h, h => src.played -= h);
    }

    public static IObservable<PlayableDirector> PausedAsObservable(this PlayableDirector src)
    {
        return Observable.FromEvent<PlayableDirector>(h => src.paused += h, h => src.paused -= h);
    }

    public static IObservable<PlayableDirector> StoppedAsObservable(this PlayableDirector src)
    {
        return Observable.FromEvent<PlayableDirector>(h => src.stopped += h, h => src.stopped -= h);
    }

    ///<summary>
    ///WrapMode can be used only when the None. 
    //Regarded as Stream until the end from///playback. 
    ///stops at Dispose 
    //to restart if///playing. 
    ///</summary>

    public static IObservable<PlayableDirector> PlayAsStream(this PlayableDirector src)
    {
        return Observable.Create<PlayableDirector>(observer =>
        {
            if (src.state == PlayState.Playing)
                src.Stop();

            src.Play();

            var disposable = src.StoppedAsObservable()

            .Subscribe(x => {

                observer.OnNext(x);

                observer.OnCompleted();

            });

            return Disposable.Create(() =>
            {
                disposable.Dispose();

                if (src.state == PlayState.Playing)
                    src.Stop();

            });

        });

    }
}