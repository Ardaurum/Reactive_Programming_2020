using System;
using UniRx;
using UnityEngine;

namespace _04_HealthViewer
{
    public sealed class PlayerReactive : MonoBehaviour
    {
        private ReactiveProperty<float> _health;
        private Subject<Unit> _showHealth;
        private ReactiveProperty<bool> _fightMode;

        public IObservable<float> HealthObservable => _health;
        public IObservable<Unit> ShowHealth => _showHealth ?? (_showHealth = new Subject<Unit>());
        public IObservable<bool> FightMode => _fightMode;

        public float Health
        {
            get => _health.Value;
            set => _health.Value = value;
        }
        
        private void Awake()
        {
            _health = new ReactiveProperty<float>(100.0f);
            _fightMode = new ReactiveProperty<bool>(false);
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                _showHealth?.OnNext(Unit.Default);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                _fightMode.Value = _fightMode.Value == false;
            }
        }
    }
}