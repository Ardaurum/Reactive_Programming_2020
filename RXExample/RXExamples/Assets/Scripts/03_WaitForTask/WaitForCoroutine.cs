using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Runner;
using UnityEngine;
using Utilities;

namespace _02_WaitForTask
{
    public sealed class WaitForCoroutine : IRunnable
    {
        private volatile bool _value;

        private void Do()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                _value = true;
            });
        }

        public IEnumerator GetValue()
        {
            Do();
            while (_value == false)
            {
                //Tyle zmarnowanych cykli :(
                yield return null;
            }
            Debug.Log($"Got Value: {Thread.CurrentThread.ManagedThreadId}!");
        }
        
        public void Run()
        {
            CoroutineDispatcher.Instance.StartCoroutine(GetValue());
        }
    }
}
