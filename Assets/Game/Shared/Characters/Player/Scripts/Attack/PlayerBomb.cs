using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Player
{
    public class PlayerBomb : MonoBehaviour
    {
        [SerializeField] private Transform[] bombBlastTransforms;
        [SerializeField] private SpriteRenderer[] bombSpriteRenderers;

        private void Start()
        {
            StartCoroutine(BombCoroutine());
        }

        private IEnumerator BombCoroutine()
        {
            foreach (Transform bombBlastTransform in bombBlastTransforms)
            {
                bombBlastTransform.DOScale(1.5f, 5).SetEase(Ease.InOutElastic);
            }

            yield return new WaitForSeconds(4.75f);
            
            foreach (SpriteRenderer spriteRenderer in bombSpriteRenderers)
            {
                Color initialColor = spriteRenderer.color;
                Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
                spriteRenderer.DOColor(newColor, 0.5f);
            }
            
            yield return new WaitForSeconds(0.5f);
            
            Destroy(this.gameObject);
        }
    }
}