using _04_HealthViewer;
using UnityEngine;

namespace Utilities
{
    public sealed class FightIndicatorCallback : MonoBehaviour
    {
        [SerializeField] private PlayerCallback _player;

        public void Awake()
        {
            _player.FightModeChanged += OnFightModeChanged;
            OnFightModeChanged(_player.FightMode);
        }

        public void OnDestroy()
        {
            _player.FightModeChanged -= OnFightModeChanged;
        }

        private void OnFightModeChanged(bool fightMode)
        {
            gameObject.SetActive(fightMode);
        }
    }
}