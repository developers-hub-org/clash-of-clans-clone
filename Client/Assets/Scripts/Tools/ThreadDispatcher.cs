using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ThreadDispatcher : MonoBehaviour
{

	private static ThreadDispatcher _instance = null;
	private static readonly Queue<Action> _queue = new Queue<Action>();
	public static ThreadDispatcher instance { get { return _instance; } }

	[RuntimeInitializeOnLoadMethod] private static void Initialize()
	{
		if (_instance == null)
		{
			_instance = new GameObject("ThreadDispatcher").AddComponent<ThreadDispatcher>();
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		_queue.Clear();
	}

	private void OnDestroy()
	{
		if(_instance == this)
        {
			_instance = null;
		}
	}

	private void Update()
	{
		lock (_queue)
		{
			while (_queue.Count > 0)
			{
				_queue.Dequeue().Invoke();
			}
		}
	}

	public void Enqueue(IEnumerator action)
	{
		lock (_queue)
		{
			_queue.Enqueue(() => 
			{
				StartCoroutine(action);
			});
		}
	}

	public void Enqueue(Action action)
	{
		Enqueue(WrappeAction(action));
	}

	public Task EnqueueAsync(Action action)
	{
		var task = new TaskCompletionSource<bool>();
		void WrappedAction()
		{
			try
			{
				action();
				task.TrySetResult(true);
			}
			catch (Exception ex)
			{
				task.TrySetException(ex);
			}
		}
		Enqueue(WrappeAction(WrappedAction));
		return task.Task;
	}

	private IEnumerator WrappeAction(Action action)
	{
		action();
		yield return null;
	}

}