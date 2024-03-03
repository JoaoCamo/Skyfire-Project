using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Game.Static.Events;

namespace Game.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private TextMeshProUGUI powerValueText;
        [SerializeField] private Image[] heartImages;
        [SerializeField] private Sprite[] heartSprites;

        private float _highScore = 0;
        private float _currentScore = 0;
        private float _scoreTargetValue = 0;

        private void Awake()
        {
            GetHighScore();
            UpdateLives(2);
        }

        private void OnEnable()
        {
            GameEvents.OnPointsValueChange += UpdateScore;
            GameEvents.OnPowerValueChange += UpdatePowerText;
            GameEvents.OnHealthValueChange += UpdateLives;
        }

        private void OnDisable()
        {
            GameEvents.OnPointsValueChange -= UpdateScore;
            GameEvents.OnPowerValueChange -= UpdatePowerText;
            GameEvents.OnHealthValueChange -= UpdateLives;
        }

        private void UpdateScore(int valueToAdd)
        {
            _scoreTargetValue += valueToAdd;
            float duration = _scoreTargetValue - _currentScore >= 500 ? 2 : 0.5f;

            if (_scoreTargetValue > _highScore)
                DOTween.To(() => _highScore, x =>
                {
                    _highScore = x;
                    highScoreText.text = _highScore.ToString("0");
                }, _scoreTargetValue, duration).SetEase(Ease.Linear);

            DOTween.To(() => _currentScore, x =>
            {
                _currentScore = x;
                currentScoreText.text = _currentScore.ToString("0");
            }, _scoreTargetValue, duration).SetEase(Ease.Linear);
        }

        private void UpdatePowerText(float value)
        {
            powerValueText.text = value.ToString("0.00") + " / 4.00";
        }

        private void UpdateLives(int currentValue)
        {
            for (int i = 0; i < heartImages.Length; i++)
                heartImages[i].sprite = i < currentValue ? heartSprites[0] : heartSprites[1];
        }

        private void GetHighScore()
        {
            
        }
    }
}
