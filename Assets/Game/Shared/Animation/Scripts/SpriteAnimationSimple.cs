using System.Collections;
using UnityEngine;

namespace Game.Animation
{
    public class SpriteAnimationSimple : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int framesPerSecond;
        [SerializeField] private bool playOnEnable;

        private WaitForSeconds _animationDelay;
        private SpriteRenderer _spriteRenderer;
        private bool _play = false;

        private Coroutine _animationCoroutine;

        private void Awake()
        {
            float timeDelay = 1f / framesPerSecond;

            _animationDelay = new WaitForSeconds(timeDelay);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _play = true;
        }

        private void OnEnable()
        {
            if(playOnEnable) StartAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        public void StartAnimation()
        {
            StopAnimation();
            _animationCoroutine = StartCoroutine(Animate());
        }

        public void StopAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
        }

        private IEnumerator Animate()
        {
            while (_play)
            {
                foreach (Sprite sprite in sprites)
                {
                    _spriteRenderer.sprite = sprite;
                    yield return _animationDelay;
                }
            }
        }
    }
}