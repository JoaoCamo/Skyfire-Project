using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.Gameplay.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image healthBarUp;
        [SerializeField] private Image healthBarDown;

        private readonly Color32[] _healthBarColor = { new Color32(255, 0, 0, 255), new Color32(255, 99, 71, 255), new Color32(255, 165, 0, 255), new Color32(255, 255, 0, 255), new Color32(0, 170, 0, 255), new Color32(144, 238, 144, 255), new Color32(128, 0, 128, 255), new Color32(216, 191, 216, 255), new Color32(0, 0, 255, 255), new Color32(173, 216, 230, 255) };

        public static Action<bool> ToggleHealthBar { private set; get; }
        public static Action<int, int, float> RequestHealthBarChange { private set; get; }
        public static Action<int, bool> RequestHealthBarColorChange { private set; get; }

        private void OnEnable()
        {
            ToggleHealthBar += Toggle;
            RequestHealthBarChange += ChangeBarFill;
            RequestHealthBarColorChange += SetHealthBarColors;
        }

        private void OnDisable()
        {
            ToggleHealthBar -= Toggle;
            RequestHealthBarChange -= ChangeBarFill;
            RequestHealthBarColorChange -= SetHealthBarColors;
        }

        private void Toggle(bool state)
        {
            int value = state ? 1 : 0;
            canvasGroup.DOFade(value, 0.5f).SetEase(Ease.Linear);
            healthBarUp.transform.DOScaleX(value, value).SetEase(Ease.Linear);
            healthBarDown.transform.DOScaleX(value, value).SetEase(Ease.Linear);
        }

        private void ChangeBarFill(int maxHealth, int currentHealth, float duration)
        {
            float ratio = (float)currentHealth / (float)maxHealth;
            healthBarUp.transform.DOScaleX(ratio, duration).SetEase(Ease.Linear);
        }

        private void SetHealthBarColors(int remainingHealthBars, bool isMidBattle)
        {
            if (isMidBattle)
            {
                healthBarUp.transform.DOKill();
                healthBarUp.transform.localScale = Vector3.one;
            }

            healthBarUp.color = _healthBarColor[remainingHealthBars];
            healthBarDown.color = remainingHealthBars - 1 >= 0 ? _healthBarColor[remainingHealthBars-1] : new Color32(0,0,0,0);
        }
    }
}