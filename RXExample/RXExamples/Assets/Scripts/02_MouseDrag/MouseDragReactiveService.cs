using System;
using _01_WaitForMouse;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;

namespace _03_MouseDrag
{
    public sealed class MouseDragReactiveHandler : IDisposable
    {
        private readonly IDisposable _subscription;
        
        public MouseDragReactiveHandler()
        {
            _subscription = MouseDragReactiveService.Instance.Drag.Subscribe((drag) =>
            {
                Debug.Log($"Mouse Reactive Drag Start: {drag.Start} => {drag.End}");
            });
        }
        
        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
    
    [RequireComponent(typeof(MouseReactiveService))]
    public sealed class MouseDragReactiveService : MonoBehaviour
    {
        #region SINGLETON
        public static MouseDragReactiveService Instance { get; private set; }
        private MouseReactiveService _mouseService;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }

            _mouseService = GetComponent<MouseReactiveService>();
            Instance = this;
        }
        #endregion

        [SerializeField] private float _deadZone;
        [SerializeField] private float _timeout;

        public IObservable<Drag> Drag;

        private void Start()
        {
            IObservable<MouseEvent> downStream = _mouseService
                .DownAsObservable()
                .Select(down => new MouseEvent(Time.unscaledTime, down));

            Drag = _mouseService
                .UpAsObservable()
                .WithLatestFrom(downStream, (up, down) => new Drag(down.Timestamp, down.Position, up))
                .Where(drag =>
                    Time.unscaledTime - drag.TimeStart > _timeout &&
                    (drag.End - drag.Start).sqrMagnitude > _deadZone * _deadZone);
        }
    }

    public readonly struct Drag
    {
        public readonly float TimeStart;
        public readonly Vector2 Start;
        public readonly Vector2 End;

        public Drag(float timeStart, Vector2 start, Vector2 end)
        {
            TimeStart = timeStart;
            Start = start;
            End = end;
        }
    }

    public readonly struct MouseEvent
    {
        public readonly float Timestamp;
        public readonly Vector2 Position;

        public MouseEvent(float timestamp, Vector2 position)
        {
            Timestamp = timestamp;
            Position = position;
        }
    }
}