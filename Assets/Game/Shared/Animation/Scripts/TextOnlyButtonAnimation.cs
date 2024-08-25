using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Game.Animation
{
    public class TextOnlyButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        private Button _button;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            _button = GetComponent<Button>();
            textMesh.color = _disabledColor;
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
            if(_button.interactable)
                textMesh.color = _activatedColor;
        }

        private void HoverExit()
        {
            if(_button.interactable)
                textMesh.color = _disabledColor;
        }
    }
}