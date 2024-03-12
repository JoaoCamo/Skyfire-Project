using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Animation
{
    public class ShockwaveAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private readonly WaitForSeconds _delay = new WaitForSeconds(0.25f);

        public void StartShockwave(float endRadius)
        {
            StartCoroutine(ShockwaveCoroutine(endRadius));
        }

        private IEnumerator ShockwaveCoroutine(float endRadius)
        {
            transform.DOScale(endRadius, 0.5f).SetEase(Ease.Linear);

            yield return _delay;

            Color originalColor = spriteRenderer.color;

            spriteRenderer.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 0), 0.25f).SetEase(Ease.Linear);

            yield return _delay;

            Destroy(gameObject);
        }
    }
}