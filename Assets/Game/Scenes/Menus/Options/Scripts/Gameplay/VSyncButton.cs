using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Game.Menus
{
    public class VSyncButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textMesh;

        private Button _button;
        private bool _isActive;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _selectedColor = new Color32(125, 160, 95, 255);
        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

        private const string V_SYNC_KEY = "V_SYNC_ON";

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            Initialize();
        }

        private void Initialize()
        {
            _isActive = PlayerPrefs.GetInt(V_SYNC_KEY, 1) == 1;
            image.color = _isActive ? _activatedColor : _disabledColor;
            textMesh.color = _isActive ? _activatedColor : _disabledColor;
        }

        private void OnClick()
        {
            _isActive = !_isActive;

            QualitySettings.vSyncCount = _isActive ? 1 : 0;
            PlayerPrefs.SetInt(V_SYNC_KEY, _isActive ? 1 : 0);

            image.color = _isActive ? _activatedColor : _disabledColor;
            textMesh.color = _isActive ? _activatedColor : _disabledColor;
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
            image.color = _selectedColor;
            textMesh.color = _selectedColor;
        }

        private void HoverExit()
        {
            image.color = _isActive ? _activatedColor : _disabledColor;
            textMesh.color = _isActive ? _activatedColor : _disabledColor;
        }
    }
}