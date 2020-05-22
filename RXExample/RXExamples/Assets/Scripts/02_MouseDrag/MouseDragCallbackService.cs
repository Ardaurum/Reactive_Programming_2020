using System;
using System.Collections;
using _01_WaitForMouse;
using UnityEngine;
using UnityEngine.Profiling;

namespace _03_MouseDrag
{
    public sealed class MouseDragCallbackHandler : IDisposable
    {
        public MouseDragCallbackHandler()
        {
            MouseDragCallbackService.Instance.MouseDrag += OnMouseDrag;
        }

        private void OnMouseDrag(Vector2 startPos, Vector2 endPos)
        {
            Debug.Log($"Mouse Callback Drag Start: {startPos} => {endPos}");
        }
        
        public void Dispose()
        {
            MouseDragCallbackService.Instance.MouseDrag -= OnMouseDrag;
        }
    }
    
    [RequireComponent(typeof(MouseCallbackService))]
    public sealed class MouseDragCallbackService : MonoBehaviour
    {
        #region SINGLETON
        public static MouseDragCallbackService Instance { get; private set; }
        private MouseCallbackService _mouseService;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
            _mouseService = GetComponent<MouseCallbackService>();
        }
        #endregion

        [SerializeField] private float _deadZone;
        [SerializeField] private float _timeout;

        private Vector3 _startPos;
        private float _startTime;
        
        public event Action<Vector2, Vector2> MouseDrag;

        private void Start()
        {
            _mouseService.MouseDown += OnMouseDown;
        }

        private void OnMouseDown()
        {
            _startPos = Input.mousePosition;
            _startTime = Time.unscaledTime;
            
            _mouseService.MouseDown -= OnMouseDown;
            _mouseService.MouseUp += OnMouseUp;
        }

        private void OnMouseUp()
        {
            if (Time.unscaledTime - _startTime > _timeout 
                && (Input.mousePosition - _startPos).sqrMagnitude > _deadZone * _deadZone)
            {
                MouseDrag?.Invoke(_startPos, Input.mousePosition);
            }
            _mouseService.MouseUp -= OnMouseUp;
            _mouseService.MouseDown += OnMouseDown;
        }
    }
}