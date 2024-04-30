using System.Collections;
using UnityEngine;

namespace Game.Animation
{
    public class PlayerSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private Sprite[] idleSprites;
        [SerializeField] private Sprite[] sideMovementSprites;
        [SerializeField] private int framesPerSecond;

        private SpriteRenderer _spriteRenderer;
        private Coroutine _animationCoroutine;

        private WaitForSeconds _timeDelay;
        private bool _canAnimate = false;
        private int previousIndex = 0;

        private void Awake()
        {
            float delay = 1f / framesPerSecond;
            _timeDelay = new WaitForSeconds(delay);

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void StartAnimation(bool isIdle)
        {
            StopAnimation();

            _canAnimate = true;
            _animationCoroutine = StartCoroutine(Animate(isIdle ? idleSprites : sideMovementSprites));
        }

        private void StopAnimation()
        {
            _canAnimate = false;

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
        }

        private IEnumerator Animate(Sprite[] spriteSheet)
        {
            int indexToStart = previousIndex;

            while(_canAnimate)
            {
                for (int i = indexToStart; i < spriteSheet.Length; i++)
                {
                    _spriteRenderer.sprite = spriteSheet[i];
                    previousIndex = i;
                    yield return _timeDelay;
                }

                indexToStart = 0;
            }
        }
    }
}