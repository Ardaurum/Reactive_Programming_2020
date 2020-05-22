using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace Utilities
{
    public sealed class MainThreadTaskQueue : MonoBehaviour
    {
        public static MainThreadTaskQueue Instance { get; private set; }

        private void Awake()
        {
            Debug.Log($"Main Thread: {Thread.CurrentThread.ManagedThreadId}");
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        
        private readonly ConcurrentQueue<Action> _taskQueue = new ConcurrentQueue<Action>();

        public void RunTaskOnMainThread(Action task)
        {
            _taskQueue.Enqueue(task);
        }

        private void Update()
        {
            while (_taskQueue.TryDequeue(out var task))
            {
                task();
            }
        }
    }
}