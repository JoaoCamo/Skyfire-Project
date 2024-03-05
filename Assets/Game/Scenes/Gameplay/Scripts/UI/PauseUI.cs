using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
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