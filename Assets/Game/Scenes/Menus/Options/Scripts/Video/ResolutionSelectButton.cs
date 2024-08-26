using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Game.Menus
{
    public class ResolutionSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textMesh;

        private int _index;
        private bool _isSelected;
        private ScreenResolutionOption _resolution;
        private ResolutionControl _resolutionControl;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _selectedColor = new Color32(125, 160, 95, 255);
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
            _isSelected = _index == currentSelected;
            button.interactable = !_isSelected;
            image.color = _isSelected ? _activatedColor : _disabledColor;
            textMesh.color = _isSelected ? _activatedColor : _disabledColor;
        }

        private void OnClick()
        {
            _resolutionControl.CurrentSelected = _index;
            _isSelected = true;
            button.interactable = false;

            int width = _resolution.width;
            int height = _resolution.height;

            Screen.SetResolution(width, height, FullScreenMode.Windowed);

            _resolutionControl.UpdateButtons();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoverEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoverExit();
        }

        private void HoverEnter()
        {
            if (button.interactable)
            {
                image.color = _selectedColor;
                textMesh.color = _selectedColor;
            }
        }

        private void HoverExit()
        {
            if (button.interactable)
            {
                image.color = _isSelected ? _activatedColor : _disabledColor;
                textMesh.color = _isSelected ? _activatedColor : _disabledColor;
            }
        }
    }
}