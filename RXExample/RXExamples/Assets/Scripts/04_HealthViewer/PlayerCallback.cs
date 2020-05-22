using System;
using UnityEngine;

namespace _04_HealthViewer
{
    public sealed class PlayerCallback : MonoBehaviour
    {
        private float _health;

        public float Health
        {
            get => _health;
            set
            {
                if (Math.Abs(_health - value) > Mathf.Epsilon)
                {
                    _health = value;
                    HealthChanged?.Invoke(value);
                }
            }
        }

        public bool FightMode { get; private set; }

        public event Action<float> HealthChanged;
        public event Action ShowHealth;
        public event Action<bool> FightModeChanged;


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ShowHealth?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                FightMode = FightMode == false;
                FightModeChanged?.Invoke(FightMode);
            }
        }
    }
}