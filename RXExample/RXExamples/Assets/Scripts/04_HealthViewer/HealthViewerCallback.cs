using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _04_HealthViewer
{
    public sealed class HealthViewerCallback : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private float _healthBlinkThreshold;
        [SerializeField] private float _fightStillTimeout;
        [SerializeField] private float _moveThreshold;
        [SerializeField] private GameObject _healthVisual;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [SerializeField] private PlayerCallback _player;

        private float _blink;
        private bool _isBlinking;

        private Coroutine _hideCoroutine;
        private Vector3 _prevPos;
        
        private void Start()
        {
            _healthSlider.enabled = false;
            _player.FightModeChanged += OnFightModeChanged;
            _player.HealthChanged += OnHealthChanged;
            _player.ShowHealth += OnShowHealth;
            _prevPos = _player.transform.position;
            
            OnFightModeChanged(_player.FightMode);
            OnHealthChanged(_player.Health);
        }

        private void OnDestroy()
        {
            _player.FightModeChanged -= OnFightModeChanged;
            _player.HealthChanged -= OnHealthChanged;
            _player.ShowHealth -= OnShowHealth;
        }

        private void OnFightModeChanged(bool fightMode)
        {
            if (_player.Health >= _healthBlinkThreshold)
            {
                _healthVisual.SetActive(fightMode);
                return;
            }
            
            if (fightMode == false)
            {
                StopHide();
            }
            else
            {
                RestartHideCoroutine();
            }
        }

        private void StopHide()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }
        }

        private void OnShowHealth()
        {
            _healthVisual.SetActive(true);
            RestartHideCoroutine();
        }
        
        private void OnHealthChanged(float health)
        {
            _healthSlider.value = health / 100.0f;
            if (health < _healthBlinkThreshold && _isBlinking == false)
            {
                _isBlinking = true;
                _healthVisual.SetActive(true);
                StartCoroutine(Blink());
                StopHide();
            }
            else if (health > _healthBlinkThreshold)
            {
                _isBlinking = false;

                _healthVisual.SetActive(true);
                RestartHideCoroutine();
            }
        }
        
        private void Update()
        {
            if ((_player.transform.position - _prevPos).sqrMagnitude >= _moveThreshold * _moveThreshold)
            {
                if (_player.FightMode)
                {
                    _healthVisual.SetActive(true);
                    RestartHideCoroutine();
                }

                _prevPos = _player.transform.position;
            }
        }
        
        private void RestartHideCoroutine()
        {
            if (_player.Health < _healthBlinkThreshold)
            {
                return;
            }

            if (_healthVisual.activeSelf)
            {
                StopHide();
                _hideCoroutine = StartCoroutine(HideTimeout());
            }
        }

        private IEnumerator HideTimeout()
        {
            var startTime = Time.time;
            while (Time.time - startTime < _fightStillTimeout)
            {
                var elapsed = Time.time - startTime;
                _canvasGroup.alpha = 1.0f - (elapsed / _fightStillTimeout);
                yield return null;
            }
            _healthVisual.SetActive(false);
            _canvasGroup.alpha = 1.0f;
        }

        private IEnumerator Blink()
        {
            while (_isBlinking)
            {
                _blink = (Mathf.Sin(Time.time * 5.0f) + 1.0f) * 0.5f;
                _canvasGroup.alpha = _blink;
                yield return null;
            }

            _canvasGroup.alpha = 1.0f;
            RestartHideCoroutine();
        }
    }
}