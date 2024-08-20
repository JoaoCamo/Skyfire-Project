using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Game.Static.Events;
using Game.Static;
using Game.Player;

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

        private void Awake()
        {
            ResetScore();
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
            GameInfo.CurrentScore += valueToAdd;
            currentScoreText.text = GameInfo.CurrentScore.ToString();

            if (GameInfo.CurrentScore > GameInfo.CurrentHighScore)
            {
                GameInfo.CurrentHighScore = GameInfo.CurrentScore;
                highScoreText.text = GameInfo.CurrentHighScore.ToString();
            }

            PlayerHealth.RequestCheckForExtraLife(GameInfo.CurrentScore);
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

            GameInfo.CurrentHighScore = GameDataManager.GetHighScore(GameInfo.PlayerType);
            highScoreText.text = GameInfo.CurrentHighScore.ToString();
        }

        private void ResetScore()
        {
            GameInfo.CurrentScore = 0;
            currentScoreText.text = GameInfo.CurrentScore.ToString();
            GetHighScore();
        }
    }
}
