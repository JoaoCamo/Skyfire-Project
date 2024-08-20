using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Game.Gameplay.UI
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        private CanvasGroup _canvasGroup;

        private readonly WaitForSeconds _textFadeStartDelay = new WaitForSeconds(1);
        private readonly WaitForSeconds _textFadeDelay = new WaitForSeconds(1);

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetText(Vector3 position, string textToDisplay, float fontSize, Color color)
        {
            textMesh.text = textToDisplay;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            transform.position = position;

            _canvasGroup.alpha = 1;

            gameObject.SetActive(true);
            
            StartCoroutine(TextHideCoroutine());
        }

        private IEnumerator TextHideCoroutine()
        {
            yield return _textFadeStartDelay;
            _canvasGroup.DOFade(0, 1).SetEase(Ease.InQuad);
            yield return _textFadeDelay;
            gameObject.SetActive(false);
        }
    }
}