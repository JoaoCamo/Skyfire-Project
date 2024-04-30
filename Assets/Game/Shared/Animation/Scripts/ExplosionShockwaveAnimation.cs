using System.Collections;
using UnityEngine;

namespace Game.Animation
{
    public class ExplosionShockwaveAnimation : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int framesPerSecond;

        private WaitForSeconds _animationDelay;
        private SpriteRenderer _spriteRenderer;

        private Coroutine _animationCoroutine;


        private void Awake()
        {
            float timeDelay = 1f / framesPerSecond;

            _animationDelay = new WaitForSeconds(timeDelay);
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        public void StartAnimation(Vector3 position)
        {
            StopAnimation();

            transform.position = position;
            gameObject.SetActive(true);
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

            _spriteRenderer.sprite = null;
            gameObject.SetActive(false);
        }
    }
}