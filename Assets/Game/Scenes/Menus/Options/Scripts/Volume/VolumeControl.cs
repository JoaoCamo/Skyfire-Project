using UnityEngine;

namespace Game.Audio
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private VolumeControlGroup volumeControlGroupPrefab;
        [SerializeField] private Transform parentTransform;

        private void Awake()
        {
            Instantiate(volumeControlGroupPrefab, parentTransform).Initialize(
                () => SetAudioGroupVolume(AudioManager.MUSIC_KEY, (PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f) + 0.1f)),
                () => SetAudioGroupVolume(AudioManager.MUSIC_KEY, (PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f) - 0.1f)),
                AudioManager.MUSIC_KEY,
                "Music"
                );

            Instantiate(volumeControlGroupPrefab, parentTransform).Initialize(
                () => SetAudioGroupVolume(AudioManager.SFX_KEY, (PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f) + 0.1f)),
                () => SetAudioGroupVolume(AudioManager.SFX_KEY, (PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f) - 0.1f)),
                AudioManager.SFX_KEY,
                "Sound Effects"
                );
        }

        private void SetAudioGroupVolume(string audioGroupKey, float value)
        {
            value = value <= AudioManager.MIN_VOL ? AudioManager.MIN_VOL : value;
            value = value >= AudioManager.MAX_VOL ? AudioManager.MAX_VOL : value;

            PlayerPrefs.SetFloat(audioGroupKey, value);
            AudioManager.SetAudioGroupVolume(audioGroupKey, value);
        }
    }
}