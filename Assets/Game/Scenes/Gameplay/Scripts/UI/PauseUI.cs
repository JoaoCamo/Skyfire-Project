using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Game.Navigation;
using Game.Player.Controls;

namespace Game.Gameplay.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button returnToMainMenuButton;

        private InputAction _pauseInput;
        private bool _isPaused = false;

        private void Awake()
        {
            _pauseInput = new PlayerControls().Player.Pause;
            _pauseInput.performed += TogglePauseInputAction;

            resumeButton.onClick.AddListener(TogglePause);
            
            restartButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                NavigationController.RequestSceneLoad(Scenes.Gameplay, LoadSceneMode.Single, true);
            });
            
            returnToMainMenuButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                NavigationController.RequestSceneLoad(Scenes.MainMenu, LoadSceneMode.Single, true);
            });
        }

        private void OnEnable()
        {
            _pauseInput.Enable();
        }

        private void OnDisable()
        {
            _pauseInput.Disable();
        }

        private void TogglePauseInputAction(InputAction.CallbackContext callbackContext)
        {
            TogglePause();
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;

            canvasGroup.DOFade(_isPaused ? 1 : 0, 0.25f).SetUpdate(isIndependentUpdate: true);
            canvasGroup.interactable = _isPaused;
            canvasGroup.blocksRaycasts = _isPaused;

            Time.timeScale = _isPaused ? 0 : 1;
        }
    }
}