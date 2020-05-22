using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace _01_WaitForMouse
{
    public sealed class MouseCallbackService : MonoBehaviour
    {
        #region SINGLETON
        public static MouseCallbackService Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }
        #endregion
        
        public event Action MouseDown;
        public event Action MouseUp;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseDown?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseUp?.Invoke();
            }
        }
    }
    
    public sealed class MouseCallback : IDisposable
    {
        public MouseCallback()
        {
            MouseCallbackService.Instance.MouseUp += MouseUp;
        }
        
        private void MouseUp()
        {
            Debug.Log("Mouse Callback: I'm clicked!");
        }

        public void Dispose()
        {
            MouseCallbackService.Instance.MouseUp -= MouseUp;
        }
    }
}