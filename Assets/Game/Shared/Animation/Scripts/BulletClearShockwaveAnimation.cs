using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Animation
{
    public class BulletClearShockwaveAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private readonly WaitForSeconds _delay = new WaitForSeconds(0.5f);

        public void StartShockwave(float radius)
        {
            StartCoroutine(ShockwaveCoroutine(radius));
        }

        private IEnumerator ShockwaveCoroutine(float radius)
        {
            transform.DOScale(radius, 0.75f).SetEase(Ease.Linear);

            yield return _delay;

            Color originalColor = spriteRenderer.color;

            spriteRenderer.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 0), 0.25f).SetEase(Ease.Linear);

            yield return _delay;

            Destroy(gameObject);
        }
    }
}