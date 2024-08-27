using UnityEngine;
using UnityEngine.UI;

namespace Game.Audio.Effects
{
    public class ButtonClickSound : MonoBehaviour
    {
        [SerializeField] private SfxTypes SfxType = SfxTypes.MenuClick;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SoundEffectController.RequestSfx(SfxType);
        }
    }
}