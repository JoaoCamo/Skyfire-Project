using System.Collections;
using UnityEngine;
using TMPro;

namespace Game.Loading
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI textMesh;
        
        private bool _canAnimate = false;
        
        private readonly WaitForSeconds _animationDelay = new WaitForSeconds(0.5f);

        private Coroutine _animationCoroutine;

        private IEnumerator AnimationCoroutine()
        {
            while (_canAnimate)
            {
                textMesh.text = "LOADING";

                yield return _animationDelay;
                
                for (int i = 0; i < 3; i++)
                {
                    textMesh.text += " .";
                    yield return _animationDelay;
                }
            }
        }

        public void ToggleLoading(bool state)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;

                _canAnimate = false;
            }
            
            if (state)
            {
                _canAnimate = true;
                _animationCoroutine = StartCoroutine(AnimationCoroutine());
            }

            canvasGroup.alpha = state ? 1 : 0;
            canvasGroup.blocksRaycasts = state;
        }
    }
}