using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Game.Static.Events;
using Game.Static;

namespace Game.Gameplay.UI
{
    public class GameStatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private RectTransform powerFillArea;
        [SerializeField] private Image[] heartImages;
        [SerializeField] private Sprite[] heartSprites;
        [SerializeField] private Image[] bombImages;
        [SerializeField] private Sprite[] bombSprites;

        private int _scoreTargetValue = 0;

        private void Awake()
        {
            GetHighScore();
        }

        private void OnEnable()
        {
            GameEvents.OnPointsValueChange += UpdateScore;
            GameEvents.OnPowerValueChange += UpdatePowerText;
            GameEvents.OnHealthValueChange += UpdateLives;
            GameEvents.OnBombValueChange += UpdateBombs;
            GameEvents.OnRetry += ResetScore;
        }

        private void OnDisable()
        {
            GameEvents.OnPointsValueChange -= UpdateScore;
            GameEvents.OnPowerValueChange -= UpdatePowerText;
            GameEvents.OnHealthValueChange -= UpdateLives;
            GameEvents.OnBombValueChange -= UpdateBombs;
            GameEvents.OnRetry -= ResetScore;
        }

        private void UpdateScore(int valueToAdd)
        {
            _scoreTargetValue += valueToAdd;
            float duration = _scoreTargetValue - GameInfo.CurrentScore >= 500 ? 2 : 0.5f;

            DOTween.To(() => GameInfo.CurrentScore, x =>
            {
                GameInfo.CurrentScore = x;
                currentScoreText.text = GameInfo.CurrentScore.ToString();

                if(GameInfo.CurrentScore > GameInfo.CurrentHighScore)
                {
                    GameInfo.CurrentHighScore = x;
                    highScoreText.text = GameInfo.CurrentHighScore.ToString();
                }

            }, _scoreTargetValue, duration).SetEase(Ease.Linear);
        }

        private void UpdatePowerText(float value)
        {
            float ratio = value / 4f;
            powerFillArea.DOScaleX(ratio, 0.25f);
        }

        private void UpdateLives(int currentValue)
        {
            for (int i = 0; i < heartImages.Length; i++)
                heartImages[i].sprite = i < currentValue ? heartSprites[0] : heartSprites[1];
        }

        private void UpdateBombs(int currentValue)
        {
            for (int i = 0; i < bombImages.Length; i++)
                bombImages[i].sprite = i < currentValue ? bombSprites[0] : bombSprites[1];
        }

        private void GetHighScore()
        {
            highScoreText.text = GameDataManager.GetHighScore(GameInfo.PlayerType).ToString();
        }

        private void ResetScore()
        {
            _scoreTargetValue = 0;
            GameInfo.CurrentScore = 0;
            GameInfo.CurrentHighScore = 0;
            currentScoreText.text = GameInfo.CurrentScore.ToString();
            GetHighScore();
        }
    }
}
