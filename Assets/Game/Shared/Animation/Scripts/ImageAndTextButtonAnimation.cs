using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Game.Animation
{
    public class ImageAndTextButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textMesh;
        private Button _button;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            _button = GetComponent<Button>();

            if (_button.interactable)
            {
                image.color = _disabledColor;
                textMesh.color = _disabledColor;
            }
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
            {
                image.color = _activatedColor;
                textMesh.color = _activatedColor;
            }
        }

        private void HoverExit()
        {
            if (_button.interactable)
            {
                image.color = _disabledColor;
                textMesh.color = _disabledColor;
            }
        }
    }
}