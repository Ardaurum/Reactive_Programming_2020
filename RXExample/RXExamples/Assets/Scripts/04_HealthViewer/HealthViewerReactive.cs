using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _04_HealthViewer
{
    public sealed class HealthViewerReactive : MonoBehaviour
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private readonly ISubject<Unit> _move = new Subject<Unit>();

        private float _blink;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fightStillTimeout;
        [SerializeField] private float _healthBlinkThreshold;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private GameObject _healthVisual;
        private Coroutine _hideCoroutine;
        [SerializeField] private float _hideTime;
        private bool _isBlinking;
        [SerializeField] private float _moveThreshold;

        [SerializeField] private PlayerReactive _player;
        private Vector3 _prevPos;

        private void Start()
        {
            _healthSlider.gameObject.SetActive(false);
            
            IObservable<float> healthStream = _player.HealthObservable;
            healthStream.Select(health => health / 100.0f)
                .Subscribe(health => _healthSlider.value = health)
                .AddTo(_disposables);
            
            IObservable<bool> fightStream = _player.FightMode;
            IObservable<Unit> moveStream = _move.AsObservable();

            IObservable<Unit> showFightStream = fightStream
                .CombineLatest(moveStream.StartWith(Unit.Default), (fight, move) => fight)
                .Where(value => value)
                .Select(value => Unit.Default);

            IObservable<bool> showHealthStream = _player.ShowHealth
                .Merge(healthStream.Select(h => Unit.Default), showFightStream)
                .Select(
                    show => Observable.Timer(TimeSpan.FromSeconds(_fightStillTimeout))
                    .Select(t => false)
                    .StartWith(true)
                ).Skip(1).Switch().DistinctUntilChanged();

            IObservable<bool> blinkingStream = _player.HealthObservable
                .Select(health => health < _healthBlinkThreshold)
                .DistinctUntilChanged();

            blinkingStream.Select(blink => blink ? Observable.Empty<bool>() : showHealthStream)
                .Switch()
                .Subscribe(SetHealthVisibility).AddTo(_disposables);
            
            blinkingStream.Subscribe(CheckBlink).AddTo(_disposables);
        }

        private void CheckBlink(bool blink)
        {
            _isBlinking = blink;
            if (blink)
            {
                StartCoroutine(Blink());
            }
        }

        private void SetHealthVisibility(bool visible)
        {
            if (visible == false)
            {
                if (_hideCoroutine == null)
                {
                    _hideCoroutine = StartCoroutine(Hide());
                }
            }
            else
            {
                if (_hideCoroutine != null)
                {
                    StopCoroutine(_hideCoroutine);
                    _hideCoroutine = null;
                }

                _healthVisual.SetActive(true);
                _canvasGroup.alpha = 1.0f;
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _disposables.Count; i++)
            {
                _disposables[i].Dispose();
            }
            _disposables.Clear();
        }

        private void Update()
        {
            if ((_player.transform.position - _prevPos).sqrMagnitude >= _moveThreshold * _moveThreshold)
            {
                _move.OnNext(Unit.Default);
                _prevPos = _player.transform.position;
            }
        }

        private IEnumerator Hide()
        {
            var startTime = Time.time;
            while (Time.time - startTime < _hideTime)
            {
                var elapsed = Time.time - startTime;
                _canvasGroup.alpha = 1.0f - elapsed / _hideTime;
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
        }
    }
}