using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Audio
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private bool _isPlaying = false;

        private readonly WaitForSeconds _audioFadeDelay = new WaitForSeconds(1);
        private const float AUDIO_FADE_DURATION = 1;

        public static Action<AudioClip> RequestNewMusic { private set; get; }
        public static Action RequestStopMusic { private set; get; }
        public static Action<bool> ToggleMusic { private set; get; }

        private void OnEnable()
        {
            RequestNewMusic += StartMusicChange;
            RequestStopMusic += StopMusic;
            ToggleMusic += PauseMusic;
        }

        private void OnDisable()
        {
            RequestNewMusic -= StartMusicChange;
            RequestStopMusic -= StopMusic;
            ToggleMusic -= PauseMusic;
        }

        private void StartMusicChange(AudioClip audioClip)
        {
            if (audioClip == null)
                return;

            if(audioClip == audioSource.clip)
            {
                if(!_isPlaying)
                    audioSource.Play();
                else
                    return;
            }

            StartCoroutine(StartMusic(audioClip));
        }

        private void StopMusic()
        {
            StartCoroutine(StopMusicCoroutine());
        }

        private IEnumerator StartMusic(AudioClip audioClip)
        {
            if(_isPlaying)
            {
                yield return StartCoroutine(StopMusicCoroutine());
            }

            audioSource.clip = audioClip;
            audioSource.Play();

            audioSource.DOFade(0.75f, AUDIO_FADE_DURATION).SetEase(Ease.Linear);

            _isPlaying = true;
        }

        private IEnumerator StopMusicCoroutine()
        {
            audioSource.DOFade(0, AUDIO_FADE_DURATION).SetEase(Ease.Linear);
            yield return _audioFadeDelay;
            audioSource.Stop();
            _isPlaying = false;
        }

        private void PauseMusic(bool state)
        {
            _isPlaying = !state;

            if (state)
                audioSource.Pause();
            else
                audioSource.Play();
        }
    }
}