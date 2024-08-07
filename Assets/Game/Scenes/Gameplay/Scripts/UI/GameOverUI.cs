using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Game.Navigation;
using Game.Static;
using Game.Static.Events;
using Game.Gameplay.StageEffects;

namespace Game.Gameplay.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button returnButton;

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
                Time.timeScale = 1;
                NavigationController.RequestSceneLoad(Scenes.Gameplay, LoadSceneMode.Single, true);
            });

            returnButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                NavigationController.RequestSceneLoad(Scenes.AddScores, LoadSceneMode.Single, true);
            });
        }

        private void GameOverToggle()
        {
            GameEvents.TogglePlayerInputs?.Invoke(_loseScreenOpen);

            retryButton.interactable = GameInfo.RetryCount > 0;

            _loseScreenOpen = !_loseScreenOpen;

            StageEffectsController.ToggleMusic(_loseScreenOpen);

            canvasGroup.DOFade(_loseScreenOpen ? 1 : 0, 0.25f).SetUpdate(isIndependentUpdate: true);
            canvasGroup.interactable = _loseScreenOpen;
            canvasGroup.blocksRaycasts = _loseScreenOpen;

            Time.timeScale = _loseScreenOpen ? 0 : 1;
        }
    }
}