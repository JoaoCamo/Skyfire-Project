using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Gameplay.StageEffects
{
    public class StageEffectsController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer continuationBackground;
        [SerializeField] private SpriteRenderer cloudBackground;
        [SerializeField] private SpriteRenderer cloudContinuationBackground;
        [SerializeField] private Sprite[] stageBackgroundSprites;
        [SerializeField] private Sprite[] cloudSprites;
        [SerializeField] private AudioSource audioSource;

        private bool _canAnimate = true;
        private Coroutine _animationCoroutine;
        private Coroutine _cloudAnimationCoroutine;

        private readonly Vector2 _initialPosition = new Vector2(0, 0);
        private readonly Vector2 _finalPosition = new Vector2(0, -2.25f);
        private readonly Vector2 _cloudInitialPosition = new Vector2(0,2);
        private readonly Vector2 _cloudFinalPosition = new Vector2(0, -5);
        private readonly WaitForSeconds _animationDelay = new WaitForSeconds(10);
        private readonly WaitForSeconds _cloudAnimationDelay = new WaitForSeconds(2.5f);
        private readonly WaitForSeconds _audioFadeDelay = new WaitForSeconds(0.5f);
        private const float ANIMATION_DURATION = 10;
        private const float CLOUD_ANIMATION_DURATION = 2.5f;
        private const float AUDIO_FADE_DURATION = 0.5f;

        public static Action<bool> ToggleMusic { private set; get; }

        private void OnEnable()
        {
            ToggleMusic += PauseMusic;
        }

        private void OnDisable()
        {
            ToggleMusic -= PauseMusic;
        }

        public void SetMusic(AudioClip clip)
        {
            if (clip == null) return;

            StartCoroutine(StartStageMusic(clip));
        }

        public void StartAnimation(int currentStage)
        {
            StopBackgroundAnimation();
            background.sprite = stageBackgroundSprites[currentStage];
            continuationBackground.sprite = stageBackgroundSprites[currentStage];
            _animationCoroutine = StartCoroutine(Animate());
        }

        public void StartCloudAnimation()
        {
            StopCloudAnimation();
            _cloudAnimationCoroutine = StartCoroutine(CloudAnimation());
        }

        private IEnumerator Animate()
        {
            Transform backgroundTransform = background.transform;
            backgroundTransform.localPosition = _initialPosition;

            while(_canAnimate)
            {
                backgroundTransform.DOLocalMove(_finalPosition, ANIMATION_DURATION).SetEase(Ease.Linear);
                yield return _animationDelay;
                backgroundTransform.localPosition = _initialPosition;
            }
        }

        private IEnumerator CloudAnimation()
        {
            Transform cloudTransform = cloudBackground.transform;
            cloudTransform.localPosition = _cloudInitialPosition;

            while (_canAnimate)
            {
                cloudBackground.sprite = cloudSprites[UnityEngine.Random.Range(0, cloudSprites.Length)];
                cloudContinuationBackground.sprite = cloudSprites[UnityEngine.Random.Range(0, cloudSprites.Length)];

                cloudTransform.DOLocalMove(_cloudFinalPosition, CLOUD_ANIMATION_DURATION).SetEase(Ease.Linear);
                yield return _cloudAnimationDelay;
                cloudTransform.localPosition = _cloudInitialPosition;
            }
        }

        private void StopBackgroundAnimation()
        {
            if(_animationCoroutine != null)
            {
                StopCoroutine( _animationCoroutine );
                background.transform.DOKill();
                _animationCoroutine = null;
            }
        }

        private void StopCloudAnimation()
        {
            if(_cloudAnimationCoroutine != null)
            {
                StopCoroutine( _cloudAnimationCoroutine );
                cloudBackground.transform.DOKill();
                _cloudAnimationCoroutine = null;
            }

            cloudBackground.sprite = null;
            cloudContinuationBackground.sprite = null;
        }

        private IEnumerator StartStageMusic(AudioClip clip)
        {
            audioSource.DOFade(0, AUDIO_FADE_DURATION).SetEase(Ease.Linear);
            
            yield return _audioFadeDelay;

            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            
            audioSource.DOFade(0.25f, AUDIO_FADE_DURATION).SetEase(Ease.Linear);
        }

        private void PauseMusic(bool state)
        {
            if(state)
                audioSource.Pause();
            else
                audioSource.Play();
        }
    }
}