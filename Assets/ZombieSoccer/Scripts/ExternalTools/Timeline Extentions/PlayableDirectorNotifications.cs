using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace PlayableExtensions
{
	public class PlayableDirectorNotifications : MonoBehaviour
	{
		//-----------------------------------------------------------------------------------------
		// Constants
		//-----------------------------------------------------------------------------------------

		private const double EPSILON = 0.00001;

		//-----------------------------------------------------------------------------------------
		// Events
		//-----------------------------------------------------------------------------------------

		public BetterEvent Played;
		public BetterEvent Paused;
		public BetterEvent Stopped;
		public BetterEvent Completed;

		public event EventHandler<PlayableState> StateChanged;
		public event EventHandler<DirectorWrapMode> Wrapped;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables
		//-----------------------------------------------------------------------------------------

		[SerializeField] private PlayableDirector _director = default;

		//-----------------------------------------------------------------------------------------
		// Private Fields
		//-----------------------------------------------------------------------------------------

		private Coroutine _watchRoutine;
		private double _lastTime = double.MinValue;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle Methods
		//-----------------------------------------------------------------------------------------

		private void Start()
		{
			//If the director played in awake, we might miss the played event invocation,
			//if our OnEnable is called after its Awake
			if (_director.playOnAwake)
			{
				Director_Played(_director);
			}
		}

		private void OnEnable()
		{
			_director.played += Director_Played;
			_director.paused += Director_Paused;
			_director.stopped += Director_Stopped;
		}

		private void OnDisable()
		{
			_director.played -= Director_Played;
			_director.paused -= Director_Paused;
			_director.stopped -= Director_Stopped;

			if (_watchRoutine != null) StopCoroutine(_watchRoutine);
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers
		//-----------------------------------------------------------------------------------------

		private void Director_Played(PlayableDirector director)
		{
			StateChanged?.Invoke(PlayableState.Playing);
			Played.Invoke();

			_watchRoutine = StartCoroutine(WatchDirector());
		}

		private void Director_Paused(PlayableDirector director)
		{
			StateChanged?.Invoke(PlayableState.Paused);
			Paused.Invoke();

			if (_watchRoutine != null) StopCoroutine(_watchRoutine);
		}

		private void Director_Stopped(PlayableDirector director)
		{
			StateChanged?.Invoke(PlayableState.Stopped);
			Stopped.Invoke();

			//If the delta time + the last recorded time is greater than the duration of the current playable,
			//assume the playable finished playing with a Wrap mode of None, and invoke the Completed event.
			if (_lastTime + Time.deltaTime >= director.playableAsset.duration)
			{
				_lastTime = double.MinValue;

				Completed.Invoke();
			}

			if (_watchRoutine != null) StopCoroutine(_watchRoutine);
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods
		//-----------------------------------------------------------------------------------------

		private IEnumerator WatchDirector()
		{
			_lastTime = double.MinValue;

			while (_director.time < EPSILON) yield return null;

			while (true)
			{
				//If the time is less than or equal to the last time recorded, invoke the Wrapped event
				if (_director.time - _lastTime < EPSILON)
				{
					Wrapped?.Invoke(_director.extrapolationMode);

					//If the wrap mode is set to Hold, exit the method to avoid repeatedly invoking the method 
					if (_director.extrapolationMode == DirectorWrapMode.Hold) yield break;
				}

				_lastTime = _director.time;
				yield return null;
			}
		}
	}

	public enum PlayableState
	{
		UnPlayed = 0,
		Playing = 1,
		Paused = 2,
		Stopped = 3
	}

	public delegate void EventHandler();

	public delegate void EventHandler<in T>(T value);
}