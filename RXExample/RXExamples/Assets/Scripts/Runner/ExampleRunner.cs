using System;
using UnityEngine;

namespace Runner
{
    public sealed class ExampleRunner : MonoBehaviour
    {
        [SerializeField] private ExampleType _exampleType;
        private object _currentExample;
        
        public void Run()
        {
            if (_currentExample != null && _currentExample is IDisposable disposable)
            {
                disposable.Dispose();
            }
            
            _currentExample = ExampleRepository.GetExample(_exampleType);
            if (_currentExample is IRunnable runnable)
            {
                runnable.Run();
            }
        }
    }
}