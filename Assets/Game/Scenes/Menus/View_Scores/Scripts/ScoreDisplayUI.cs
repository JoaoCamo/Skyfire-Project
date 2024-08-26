using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Saves;
using Game.Static;
using UnityEngine.EventSystems;

namespace Game.Menus
{
    public class ScoreDisplayUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameMesh;
        [SerializeField] private TextMeshProUGUI playerTypeMesh;
        [SerializeField] private TextMeshProUGUI scoreMesh;
        [SerializeField] private TextMeshProUGUI dateMesh;

        private Button _button;
        private ViewScoresController _controller;
        private ScoreData _data;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            _button = GetComponent<Button>();

            image.color = _disabledColor;
            nameMesh.color = _disabledColor;
            playerTypeMesh.color = _disabledColor;
            scoreMesh.color = _disabledColor;
            dateMesh.color = _disabledColor;
        }

        public void Initialize(ScoreData data, ViewScoresController controller)
        {
            nameMesh.text = data.name;
            playerTypeMesh.text = GameDataManager.GetPlayerTypeNames(data.playerType);
            scoreMesh.text = data.score.ToString();
            dateMesh.text = data.date;

            _data = data;
            _controller = controller;

            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _controller.DisplayScoreInfo(_data);
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
            nameMesh.color = _activatedColor;
            playerTypeMesh.color = _activatedColor;
            scoreMesh.color = _activatedColor;
            dateMesh.color = _activatedColor;
        }

        private void HoverExit()
        {
            image.color = _disabledColor;
            nameMesh.color = _disabledColor;
            playerTypeMesh.color = _disabledColor;
            scoreMesh.color = _disabledColor;
            dateMesh.color = _disabledColor;
        }
    }
}