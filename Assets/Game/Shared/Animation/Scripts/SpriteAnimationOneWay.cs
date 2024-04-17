using System.Collections;
using UnityEngine;

namespace Game.Animation
{
    public class SpriteAnimationOneWay : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int framesPerSecond;
        [SerializeField] private bool playOnEnable;

        private WaitForSeconds _animationDelay;
        private SpriteRenderer _spriteRenderer;

        private Coroutine _animationCoroutine;

        private void Awake()
        {
            float timeDelay = 1f / framesPerSecond;

            _animationDelay = new WaitForSeconds(timeDelay);
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (playOnEnable) StartAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        public void StartAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            _animationCoroutine = StartCoroutine(Animate());
        }

        private void StopAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
        }

        private IEnumerator Animate()
        {
            foreach (Sprite sprite in sprites)
            {
                _spriteRenderer.sprite = sprite;
                yield return _animationDelay;
            }
        }

        public float GetAnimationDuration()
        {
            float timeDelay = 1f / framesPerSecond;
            float totalDelay = sprites.Length * timeDelay;

            return totalDelay;
        }
    }
}