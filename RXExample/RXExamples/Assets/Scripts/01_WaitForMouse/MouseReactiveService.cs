using System;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;

namespace _01_WaitForMouse
{
    public sealed class MouseReactiveService : MonoBehaviour
    {
        #region SINGLETON
        public static MouseReactiveService Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }
        #endregion
        
        private Subject<Vector2> _mouseDownSubject;
        private Subject<Vector2> _mouseUpSubject;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownSubject?.OnNext(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _mouseUpSubject?.OnNext(Input.mousePosition);
            }
        }

        public IObservable<Vector2> DownAsObservable()
        {
            return _mouseDownSubject ?? (_mouseDownSubject = new Subject<Vector2>());
        }

        public IObservable<Vector2> UpAsObservable()
        {
            return _mouseUpSubject ?? (_mouseUpSubject = new Subject<Vector2>());
        }
    }
    
    public sealed class MouseReactive : IDisposable
    {
        private readonly IDisposable _subscription;
        
        public MouseReactive()
        {
            _subscription = MouseReactiveService.Instance.UpAsObservable().Subscribe((pos) =>
            {
                Debug.Log("Mouse Reactive: I'm clicked!");
            });
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}