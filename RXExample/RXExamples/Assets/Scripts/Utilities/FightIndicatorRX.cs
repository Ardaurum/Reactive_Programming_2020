using System;
using _04_HealthViewer;
using UnityEngine;
using UniRx;

namespace Utilities
{
    public sealed class FightIndicatorRX : MonoBehaviour
    {
        [SerializeField] private PlayerReactive _player;
        private IDisposable _subscription;

        public void Start()
        {
            _subscription = _player.FightMode.Subscribe(gameObject.SetActive);
        }

        public void OnDestroy()
        {
            _subscription?.Dispose();
        }
    }
}