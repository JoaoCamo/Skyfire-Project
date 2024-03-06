using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Gameplay.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform fillTransform;
        [SerializeField] private RectTransform healthBarIndicatorsParentRight;
        [SerializeField] private RectTransform healthBarIndicatorsParentLeft;
        [SerializeField] private GameObject healthBarIndicatorPrefab;

        private readonly List<GameObject> _healthBarIndicators = new List<GameObject>();

        public static Action<bool> ToggleHealthBar { private set; get; }
        public static Action<int, int, float> RequestHealthBarChange { private set; get; }
        public static Action<int> RequestHealthBarIndicatorsChange { private set; get; }

        private void OnEnable()
        {
            ToggleHealthBar += Toggle;
            RequestHealthBarChange += ChangeBarFill;
            RequestHealthBarIndicatorsChange += SetHealthBarIndicators;
        }

        private void OnDisable()
        {
            ToggleHealthBar -= Toggle;
            RequestHealthBarChange -= ChangeBarFill;
            RequestHealthBarIndicatorsChange -= SetHealthBarIndicators;
        }

        private void Awake()
        {
            InitiliazeIndicators();
        }

        private void InitiliazeIndicators()
        {
            for (int i = 0; i < 10; i++)
            {
                Transform parent = i%2 == 0 ? healthBarIndicatorsParentRight : healthBarIndicatorsParentLeft;
                GameObject healthBar = Instantiate(healthBarIndicatorPrefab, parent);
                _healthBarIndicators.Add(healthBar);
            }
        }

        private void Toggle(bool state)
        {
            int value = state ? 1 : 0;
            fillTransform.DOScale(value, value);
            canvasGroup.DOFade(value, 0.5f);
        }

        private void ChangeBarFill(int maxHealth, int currentHealth, float duration)
        {
            float ratio = (float)currentHealth / (float)maxHealth;
            fillTransform.DOScaleX(ratio, duration);
        }

        private void SetHealthBarIndicators(int remainingHealthBars)
        {
            int value = remainingHealthBars * 2;

            for (int i = 0; i < _healthBarIndicators.Count; i++)
            {
                _healthBarIndicators[i].SetActive(i < value);
            }
        }
    }
}