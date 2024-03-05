using System;
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

        public static Action<bool> ToggleHealthBar { private set; get; }
        public static Action<int, int, float> RequestHealthBarChange { private set; get; }

        private void OnEnable()
        {
            ToggleHealthBar += Toggle;
            RequestHealthBarChange += ChangeBarFill;
        }

        private void OnDisable()
        {
            ToggleHealthBar -= Toggle;
            RequestHealthBarChange -= ChangeBarFill;
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
    }
}