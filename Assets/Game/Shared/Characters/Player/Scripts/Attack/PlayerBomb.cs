using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Player
{
    public class PlayerBomb : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] bombSpriteRenderers;

        private readonly WaitForSeconds _bombPhaseOneDelay = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _bombPhaseTwoDelay = new WaitForSeconds(3.5f);
        private readonly WaitForSeconds _bombFadeDelay = new WaitForSeconds(0.75f);

        private void Start()
        {
            StartCoroutine(BombPhaseOne());
        }

        private IEnumerator BombPhaseOne()
        {
            transform.DOScale(0.2f, 0.5f).SetEase(Ease.Linear);

            yield return _bombPhaseOneDelay;

            StartCoroutine(BombPhaseTwo());
        }

        private IEnumerator BombPhaseTwo()
        {
            transform.DOScale(1.5f, 3.5f).SetEase(Ease.InOutExpo);

            yield return _bombPhaseTwoDelay;

            foreach (SpriteRenderer spriteRenderer in bombSpriteRenderers)
            {
                Color initialColor = spriteRenderer.color;
                Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
                spriteRenderer.DOColor(newColor, 0.5f);
            }
            
            yield return _bombFadeDelay;
            
            Destroy(this.gameObject);
        }
    }
}