using System.Threading;
using System.Threading.Tasks;
using Runner;
using UniRx;
using UnityEngine;

namespace _02_WaitForTask
{
    public sealed class WaitForReactive : IRunnable
    {
        private bool _value;

        private Task Do()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                _value = true;
            });
        }

        public void GetValue()
        {
            Do().ToObservable().ObserveOnMainThread().Subscribe(unit =>
            {
                Debug.Log($"Get Value: {Thread.CurrentThread.ManagedThreadId}");
            });
        }
        
        public void Run()
        {
            GetValue();
        }
    }
}