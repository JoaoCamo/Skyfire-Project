using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Game.Menus
{
    public class DifficultySelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private DifficultySelectionInfo info;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textMesh;

        private DifficultySelectionController _controller;
        private Button _button;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            image.color = _disabledColor;
            textMesh.color = _disabledColor;
        }

        public void Initialize(DifficultySelectionController controller)
        {
            _controller = controller;
        }

        private void OnClick()
        {
            _controller.SelectDifficulty(info.difficultyType);
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
            image.color = _activatedColor;
            textMesh.color = _activatedColor;
            _controller.SetInfo(info.difficultyType, info.description);
        }

        private void HoverExit()
        {
            image.color = _disabledColor;
            textMesh.color = _disabledColor;
            _controller.ClearInfo();
        }
    }
}