using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Gameplay.Animation
{
    public class StageEffectsController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer continuationBackground;
        [SerializeField] private Sprite[] stageBackgroundSprites;
        [SerializeField] private AudioSource audioSource;

        private bool _canAnimate = true;
        private Coroutine _animationCoroutine;

        private readonly Vector2 _initialPosition = new Vector2(0, 0);
        private readonly Vector2 _finalPosition = new Vector2(0, -2);
        private readonly WaitForSeconds _animationDelay = new WaitForSeconds(3);
        private readonly WaitForSeconds _audioFadeDelay = new WaitForSeconds(2);
        private const float ANIMATION_DURATION = 3;
        private const float AUDIO_FADE_DURATION = 2;

        public void SetMusic(AudioClip clip)
        {
            if (clip == null) return;

            StartCoroutine(StartStageMusic(clip));
        }

        public void StartAnimation(int currentStage)
        {
            StopAnimation();
            background.sprite = stageBackgroundSprites[currentStage];
            continuationBackground.sprite = stageBackgroundSprites[currentStage];
            _animationCoroutine = StartCoroutine(Animate());
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

        private void StopAnimation()
        {
            if(_animationCoroutine != null)
            {
                StopCoroutine( _animationCoroutine );
                background.transform.DOKill();
                _animationCoroutine = null;
            }
        }

        private IEnumerator StartStageMusic(AudioClip clip)
        {
            audioSource.DOFade(0, AUDIO_FADE_DURATION).SetEase(Ease.Linear);
            
            yield return _audioFadeDelay;
            
            audioSource.clip = clip;
            audioSource.DOFade(1, AUDIO_FADE_DURATION).SetEase(Ease.Linear);
        }
    }
}