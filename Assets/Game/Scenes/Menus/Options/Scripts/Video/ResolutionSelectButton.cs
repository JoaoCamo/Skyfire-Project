using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Menus
{
    public class ResolutionSelectButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textMesh;

        private int _index;
        private ScreenResolutionOption _resolution;
        private ResolutionControl _resolutionControl;

        private readonly Color32 _activatedColor = new Color32(181, 230, 137, 255);
        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

        public void Initialize(ResolutionControl resolutionControl, ScreenResolutionOption resolution, int index)
        {
            _index = index;
            _resolution = resolution;
            _resolutionControl = resolutionControl;
            textMesh.text = resolution.width + "x" + resolution.height;
            button.onClick.AddListener(OnClick);
        }

        public void UpdateButton(int currentSelected)
        {
            image.color = _index == currentSelected ? _activatedColor : _disabledColor;
        }

        private void OnClick()
        {
            _resolutionControl.CurrentSelected = _index;

            int width = _resolution.width;
            int height = _resolution.height;

            Screen.SetResolution(width, height, FullScreenMode.Windowed);

            _resolutionControl.UpdateButtons();
        }
    }
}