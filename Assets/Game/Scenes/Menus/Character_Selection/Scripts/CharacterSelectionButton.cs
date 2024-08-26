using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Menus
{
    public class CharacterSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CharacterSelectionInfo info;
        [SerializeField] private Image image;

        private CharacterSelectionController _controller;
        private Button _button;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            image.color = _disabledColor;
        }

        public void Initialize(CharacterSelectionController controller)
        {
            _controller = controller;
        }

        private void OnClick()
        {
            _controller.SelectCharacter(info.playerType);
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
            _controller.SetInfo(info.playerType, info.description);
        }

        private void HoverExit()
        {
            image.color = _disabledColor;
            _controller.ClearInfo();
        }
    }
}