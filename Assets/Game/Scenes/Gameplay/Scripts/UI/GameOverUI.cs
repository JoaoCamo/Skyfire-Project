using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Game.Navigation;
using Game.Static;
using Game.Static.Events;

namespace Game.Gameplay.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button returnToMainMenuButton;

        private bool _loseScreenOpen = false;

        public static bool LoseScreenOpen { get; private set; }

        private void Awake()
        {
            retryButton.onClick.AddListener(() =>
            {
                GameOverToggle();
                GameEvents.OnRetry?.Invoke();
                GameInfo.RetryCount--;
            });

            restartButton.onClick.AddListener(() => NavigationController.RequestSceneLoad?.Invoke(Scenes.Gameplay, LoadSceneMode.Single, true));

            returnToMainMenuButton.onClick.AddListener(() => NavigationController.RequestSceneLoad?.Invoke(Scenes.MainMenu, LoadSceneMode.Single, true));
        }

        private void OnEnable()
        {
            GameEvents.OnGameEndLose += GameOverToggle;
        }

        private void OnDisable()
        {
            GameEvents.OnGameEndLose -= GameOverToggle;
        }

        private void GameOverToggle()
        {
            GameEvents.TogglePlayerInputs?.Invoke(_loseScreenOpen);

            retryButton.interactable = GameInfo.RetryCount > 0;

            _loseScreenOpen = !_loseScreenOpen;

            canvasGroup.DOFade(_loseScreenOpen ? 1 : 0, 0.25f).SetUpdate(isIndependentUpdate: true);
            canvasGroup.interactable = _loseScreenOpen;
            canvasGroup.blocksRaycasts = _loseScreenOpen;

            Time.timeScale = _loseScreenOpen ? 0 : 1;
        }
    }
}