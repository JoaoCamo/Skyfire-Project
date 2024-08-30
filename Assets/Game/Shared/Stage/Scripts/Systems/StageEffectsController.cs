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
        [SerializeField] private SpriteRenderer stageDecorationRenderer;
        [SerializeField] private Sprite[] stageBackgroundSprites;
        [SerializeField] private StageClutter[] decorationSprites;

        private bool _canAnimateBackground = false;
        private bool _canAnimateDecoration = false;
        private WaitForSeconds _decorationAnimationDelay;
        private Coroutine _backgroundAnimationCoroutine = null;
        private Coroutine _decorationAnimationCoroutine = null;

        private readonly Vector2 _backgroundInitialPosition = new Vector2(0, 0);
        private readonly Vector2 _backgroundFinalPosition = new Vector2(0, -2.25f);
        private readonly WaitForSeconds _backgroundAnimationDelay = new WaitForSeconds(20);
        private const float DECORATION_X_POSITION_MAX = 0.75f;
        private const float DECORATION_X_POSITION_MIN = -0.75f;
        private const float DECORATION_Y_POSITION_START = 2f;
        private const float DECORATION_Y_POSITION_FINAL = -2f;
        private const int BACKGROUND_ANIMATION_DURATION = 20;
        private const string STAGE_CLUTTER_KEY = "STAGE_CLUTTER__ON";

        public void StartAnimation(int currentStage)
        {
            StopBackgroundAnimation();
            background.sprite = stageBackgroundSprites[currentStage];
            continuationBackground.sprite = stageBackgroundSprites[currentStage];
            _backgroundAnimationCoroutine = StartCoroutine(Animate());
            StartDecorationAnimation(currentStage);
        }

        private void StartDecorationAnimation(int currentStage)
        {
            StopDecorationAnimation();
            _decorationAnimationCoroutine = StartCoroutine(DecorationAnimation(currentStage));
        }

        private IEnumerator Animate()
        {
            Transform backgroundTransform = background.transform;
            backgroundTransform.position = _backgroundInitialPosition;
            _canAnimateBackground = true;

            while(_canAnimateBackground)
            {
                backgroundTransform.DOMove(_backgroundFinalPosition, BACKGROUND_ANIMATION_DURATION).SetEase(Ease.Linear);
                yield return _backgroundAnimationDelay;
                backgroundTransform.position = _backgroundInitialPosition;
                backgroundTransform.transform.DOKill();
            }
        }

        private IEnumerator DecorationAnimation(int currentStage)
        {
            StageClutter info = decorationSprites[currentStage];

            Transform decorationTransform = stageDecorationRenderer.transform;

            float xPosition = UnityEngine.Random.Range(DECORATION_X_POSITION_MIN, DECORATION_X_POSITION_MAX);
            decorationTransform.position = new Vector3(xPosition, DECORATION_Y_POSITION_START);

            _decorationAnimationDelay = new WaitForSeconds(info.clutterAnimationDuration);

            _canAnimateDecoration = PlayerPrefs.GetInt(STAGE_CLUTTER_KEY, 1) == 1;

            while (_canAnimateDecoration)
            {
                stageDecorationRenderer.sprite = info.cluttlerSprites[UnityEngine.Random.Range(0, decorationSprites[currentStage].cluttlerSprites.Length)];
                decorationTransform.DOMove(new Vector3(xPosition, DECORATION_Y_POSITION_FINAL), info.clutterAnimationDuration).SetEase(Ease.Linear);

                yield return _decorationAnimationDelay;
                
                xPosition = UnityEngine.Random.Range(DECORATION_X_POSITION_MIN, DECORATION_X_POSITION_MAX);
                decorationTransform.position = new Vector3(xPosition, DECORATION_Y_POSITION_START);
            }
        }

        private void StopBackgroundAnimation()
        {
            if(_backgroundAnimationCoroutine != null)
            {
                StopCoroutine( _backgroundAnimationCoroutine );
                _backgroundAnimationCoroutine = null;
            }
            
            background.transform.DOKill();
            _canAnimateBackground = false;
        }

        private void StopDecorationAnimation()
        {
            if(_decorationAnimationCoroutine != null)
            {
                StopCoroutine( _decorationAnimationCoroutine );
                _decorationAnimationCoroutine = null;
            }

            _canAnimateDecoration = false;
            stageDecorationRenderer.transform.DOKill();
            stageDecorationRenderer.sprite = null;
        }
    }
}