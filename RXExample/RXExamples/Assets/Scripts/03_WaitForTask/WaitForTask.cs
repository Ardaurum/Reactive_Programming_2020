using System.Threading;
using System.Threading.Tasks;
using Runner;
using UnityEngine;
using Utilities;

namespace _02_WaitForTask
{
    public sealed class WaitForTask : IRunnable
    {
        private readonly TaskCompletionSource<bool> _value = new TaskCompletionSource<bool>();

        private Task Do()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                _value.SetResult(true);
            });
            
            return _value.Task;
        }

        public void GetValue()
        {
            Do().ContinueWith((task) =>
            {
                Debug.Log($"Get Value: {Thread.CurrentThread.ManagedThreadId}!");
                MainThreadTaskQueue.Instance.RunTaskOnMainThread(() =>
                {
                    Debug.Log($"Get Value On Main Thread: {Thread.CurrentThread.ManagedThreadId}!");
                });
            });
        }
        
        public void Run()
        {
            GetValue();
        }
    }
}