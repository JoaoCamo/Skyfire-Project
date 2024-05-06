using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Navigation;

namespace Game.Menus
{
    public class OptionsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleMesh;
        [SerializeField] private Button volumeButton;
        [SerializeField] private Button returnButton;
        [SerializeField] private CanvasGroup volumeCanvas;
        [SerializeField] private CanvasGroup mainCanvas;

        private bool _onMainPage = true;

        private void Awake()
        {
            LoadButtons();
        }

        private void LoadButtons()
        {
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
            _onMainPage = false;
            ChangeCanvas(mainCanvas, volumeCanvas, "VOLUME");
        }

        private void ReturnButtonOnClick()
        {
            if (_onMainPage)
            {
                NavigationController.RequestSceneUnload();
            }
            else
            {
                _onMainPage = true;
                ChangeCanvas(volumeCanvas, mainCanvas, "OPTIONS");
            }
        }
    }
}