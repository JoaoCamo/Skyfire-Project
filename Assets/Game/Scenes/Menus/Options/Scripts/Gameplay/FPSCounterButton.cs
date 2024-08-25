using UnityEngine;
using UnityEngine.UI;
using Game.Static;

namespace Game.Menus
{
    public class FPSCounterButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private Image image;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

        private const string SHOW_FPS_KEY = "SHOW_FPS";

        private void Awake()
        {
            image = button.GetComponent<Image>();

            button.onClick.AddListener(OnClick);
            Initialize();
        }

        private void Initialize()
        {
            bool value = PlayerPrefs.GetInt(SHOW_FPS_KEY, 0) == 1;
            GameInfo.ShowFps = value;

            image.color = value ? _activatedColor : _disabledColor;
        }

        private void OnClick()
        {
            bool value = PlayerPrefs.GetInt(SHOW_FPS_KEY, 0) == 1;
            value = !value;

            GameInfo.ShowFps = value;
            PlayerPrefs.SetInt(SHOW_FPS_KEY, value ? 1 : 0);

            image.color = value ? _activatedColor : _disabledColor;
        }
    }
}