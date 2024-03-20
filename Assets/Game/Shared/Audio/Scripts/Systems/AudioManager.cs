using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        private static AudioMixer mainAudioMixer;
        
        public static string MUSIC_KEY = "MUSIC_VOLUME";
        public static string SFX_KEY = "SFX_VOLUME";
        public static float MIN_VOL = 0.0001f;
        public static float MAX_VOL = 1f;

        private void Awake()
        {
            mainAudioMixer = audioMixer;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetAudioGroupVolume(MUSIC_KEY, PlayerPrefs.GetFloat(MUSIC_KEY, 1f));
            SetAudioGroupVolume(SFX_KEY, PlayerPrefs.GetFloat(SFX_KEY, 1f));
        }

        public static void SetAudioGroupVolume(string audioGroup, float value)
        {
            mainAudioMixer.SetFloat(audioGroup, Mathf.Log10(value) * 20);
        }
    }
}