using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Animation
{
    public class ImageOnlyButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        private Button _button;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            _button = GetComponent<Button>();

            if(_button.interactable)
                image.color = _disabledColor;
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
            if (_button.interactable)
                image.color = _activatedColor;
        }

        private void HoverExit()
        {
            if (_button.interactable)
                image.color = _disabledColor;
        }
    }
}