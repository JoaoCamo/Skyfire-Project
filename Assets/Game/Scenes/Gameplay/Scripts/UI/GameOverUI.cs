using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using Game.Navigation;
using Game.Static;
using Game.Static.Events;
using Game.Audio;

namespace Game.Gameplay.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI retryTextMesh;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button returnButton;

        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

        private static bool _loseScreenOpen = false;

        public static bool LoseScreenOpen => _loseScreenOpen;

        private void Awake()
        {
            LoadButtons();
        }

        private void OnEnable()
        {
            GameEvents.OnGameEndLose += GameOverToggle;
        }

        private void OnDisable()
        {
            GameEvents.OnGameEndLose -= GameOverToggle;
        }

        private void LoadButtons()
        {
            retryButton.onClick.AddListener(() =>
            {
                GameOverToggle();
                GameEvents.OnRetry();
                GameInfo.RetryCount--;
                GameInfo.usedRetry = true;
            });

            restartButton.onClick.AddListener(() =>
            {
                _loseScreenOpen = false;
                Time.timeScale = 1;
                NavigationController.RequestSceneLoad(Scenes.Gameplay, LoadSceneMode.Single, true);
            });

            returnButton.onClick.AddListener(() =>
            {
                _loseScreenOpen = false;
                Time.timeScale = 1;
                NavigationController.RequestSceneLoad(Scenes.AddScores, LoadSceneMode.Single, true);
            });
        }

        private void GameOverToggle()
        {
            GameEvents.TogglePlayerInputs?.Invoke(_loseScreenOpen);

            retryButton.interactable = GameInfo.RetryCount > 0;
            retryTextMesh.color = GameInfo.RetryCount > 0 ? retryTextMesh.color : _disabledColor;

            _loseScreenOpen = !_loseScreenOpen;

            MusicController.ToggleMusic(_loseScreenOpen);

            canvasGroup.DOFade(_loseScreenOpen ? 1 : 0, 0.25f).SetUpdate(isIndependentUpdate: true);
            canvasGroup.interactable = _loseScreenOpen;
            canvasGroup.blocksRaycasts = _loseScreenOpen;

            Time.timeScale = _loseScreenOpen ? 0 : 1;
        }
    }
}