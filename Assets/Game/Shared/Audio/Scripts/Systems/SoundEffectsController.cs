using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class SoundEffectController : MonoBehaviour
    {
        [SerializeField] private GameObject audioSourcePrefab;
        [SerializeField] public SoundEffects soundEffectsReference;

        private readonly List<AudioSource> _audioSources = new List<AudioSource>();

        public static Action<SfxTypes> RequestSfx { private set; get; }

        private void Awake()
        {
            for (int i = 0; i < 30; i++)
            {
                AudioSource audioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();
                _audioSources.Add(audioSource);
            }
        }

        private void OnEnable()
        {
            RequestSfx += RequestSoundEffect;
        }

        private void OnDisable()
        {
            RequestSfx -= RequestSoundEffect;
        }

        private void RequestSoundEffect(SfxTypes sfxType)
        {
            AudioSource audioSource = _audioSources.Find(a => !a.isPlaying);

            if (audioSource != null)
            {
                SoundEffect soundEffect = soundEffectsReference.soundEffects[(int)sfxType];

                audioSource.clip = soundEffect.audioClips[UnityEngine.Random.Range(0, soundEffect.audioClips.Length)];
                audioSource.volume = soundEffect.volume;
                audioSource.Play();
            }
        }
    }
}