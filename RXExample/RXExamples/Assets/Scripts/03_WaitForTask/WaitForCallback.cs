using System;
using System.Threading;
using System.Threading.Tasks;
using Runner;
using UnityEngine;
using Utilities;

namespace _02_WaitForTask
{
    public sealed class WaitForCallback : IRunnable
    {
        private volatile bool _value;
        private event Action<bool> Complete;

        private void Do()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                _value = true;
                Complete?.Invoke(_value);
            });
        }

        public void GetValue()
        {
            Complete += OnCompleted;
            Do();
        }

        private void OnCompleted(bool value)
        {
            Complete -= OnCompleted;
            Debug.Log($"Got Value: {Thread.CurrentThread.ManagedThreadId}!");
            
            //Jesteśmy na innym wątku. Żeby wrócić do głównego wątku musimy wrzucić akcję na kolejkę uruchamianą na głównym wątku!
            MainThreadTaskQueue.Instance.RunTaskOnMainThread(
                () => Debug.Log($"Got Value On Main Thread: {Thread.CurrentThread.ManagedThreadId}!")
            );
        }

        public void Run()
        {
            GetValue();
        }
    }
}