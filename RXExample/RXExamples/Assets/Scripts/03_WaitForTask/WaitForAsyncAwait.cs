using System.Threading;
using System.Threading.Tasks;
using Runner;
using UnityEngine;

namespace _02_WaitForTask
{
    public sealed class WaitForAsyncAwait : IRunnable
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

        public async void GetValue()
        {
            await Do().ConfigureAwait(true);
            //Wracamy do głównego wątku dzięki `ConfigureAwait(true)`. Dzięki temu przed wywołaniem metody jest zapisywany
            //kontekst uruchomienia. Jest to wolniejsze niż wersja `ConfigureAwait(false)`, także używać w zależności co jest potrzebne!
            Debug.Log($"Get Value: {Thread.CurrentThread.ManagedThreadId}");
        }

        public void Run()
        {
            GetValue();
        }
    }
}