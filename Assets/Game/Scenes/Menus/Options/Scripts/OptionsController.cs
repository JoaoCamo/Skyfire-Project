using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Navigation;

namespace Game.Menus
{
    public class OptionsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleMesh;
        [SerializeField] private Button gameplayButton;
        [SerializeField] private Button volumeButton;
        [SerializeField] private Button returnButton;
        [SerializeField] private CanvasGroup gameplayCanvas;
        [SerializeField] private CanvasGroup volumeCanvas;
        [SerializeField] private CanvasGroup mainCanvas;

        private CanvasGroup _selectedCanvas;
        private bool _onMainPage = true;

        private void Awake()
        {
            LoadButtons();
            _selectedCanvas = mainCanvas;
        }

        private void LoadButtons()
        {
            gameplayButton.onClick.AddListener(GameplayButtonOnClick);
            volumeButton.onClick.AddListener(VolumeButtonOnClick);
            returnButton.onClick.AddListener(ReturnButtonOnClick);
        }

        private void ChangeCanvas(CanvasGroup canvasToHide, CanvasGroup canvasToShow, string newTitle)
        {
            titleMesh.text = newTitle;

            canvasToHide.alpha = 0;
            canvasToHide.interactable = false;
            canvasToHide.blocksRaycasts = false;

            canvasToShow.alpha = 1;
            canvasToShow.interactable = true;
            canvasToShow.blocksRaycasts = true;
        }

        private void VolumeButtonOnClick()
        {
            ChangeCanvas(_selectedCanvas, volumeCanvas, "VOLUME");
            _selectedCanvas = volumeCanvas;
            _onMainPage = false;
        }

        private void GameplayButtonOnClick()
        {
            ChangeCanvas(_selectedCanvas, gameplayCanvas, "GAMEPLAY");
            _selectedCanvas = gameplayCanvas;
            _onMainPage = false;
        }

        private void ReturnButtonOnClick()
        {
            if (_onMainPage)
            {
                NavigationController.RequestSceneUnload();
            }
            else
            {
                ChangeCanvas(_selectedCanvas, mainCanvas, "OPTIONS");
                _selectedCanvas = mainCanvas;
                _onMainPage = true;
            }
        }
    }
}