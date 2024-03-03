using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Player
{
    public class PlayerBomb : MonoBehaviour
    {
        [SerializeField] private Transform[] bombBlastTransforms;
        [SerializeField] private SpriteRenderer[] bombSpriteRenderers;

        private readonly WaitForSeconds _bombPhaseOneDelay = new WaitForSeconds(1);
        private readonly WaitForSeconds _bombPhaseTwoDelay = new WaitForSeconds(4.75f);
        private readonly WaitForSeconds _bombFadeDelay = new WaitForSeconds(0.5f);

        private void Start()
        {
            StartCoroutine(BombPhaseOne());
        }

        private IEnumerator BombPhaseOne()
        {
            foreach (Transform bombBlastTransform in bombBlastTransforms)
            {
                bombBlastTransform.DOScale(0.2f, 1).SetEase(Ease.Linear);
            }

            yield return _bombPhaseOneDelay;

            StartCoroutine(BombPhaseTwo());
        }

        private IEnumerator BombPhaseTwo()
        {
            foreach (Transform bombBlastTransform in bombBlastTransforms)
            {
                bombBlastTransform.DOScale(1.5f, 5).SetEase(Ease.InOutElastic);
            }

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