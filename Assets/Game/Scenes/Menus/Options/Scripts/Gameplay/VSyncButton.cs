using UnityEngine;
using UnityEngine.UI;

namespace Game.Menus
{
    public class VSyncButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private Image image;

        private readonly Color32 _activatedColor = new Color32(197, 250, 149, 255);
        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

        private const string V_SYNC_KEY = "V_SYNC_ON";

        private void Awake()
        {
            image = button.GetComponent<Image>();

            button.onClick.AddListener(OnClick);
            Initialize();
        }

        private void Initialize()
        {
            bool value = PlayerPrefs.GetInt(V_SYNC_KEY, 1) == 1;
            QualitySettings.vSyncCount = value ? 1 : 0;

            image.color = value ? _activatedColor : _disabledColor;
        }

        private void OnClick()
        {
            bool value = PlayerPrefs.GetInt(V_SYNC_KEY, 1) == 1;
            value = !value;

            QualitySettings.vSyncCount = value ? 1 : 0;
            PlayerPrefs.SetInt(V_SYNC_KEY, value ? 1 : 0);

            image.color = value ? _activatedColor : _disabledColor;
        }
    }
}