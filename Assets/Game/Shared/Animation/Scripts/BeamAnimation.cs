using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Animation
{
    public class BeamAnimation : MonoBehaviour
    {
        [SerializeField] private Sprite[] firstStageSprites;
        [SerializeField] private Sprite[] secondStageSprites;
        [SerializeField] private Sprite[] thirdStageSprites;
        [SerializeField] private int framesPerSecondFirstStage;
        [SerializeField] private int framesPerSecondSecondStage;
        [SerializeField] private int framesPerSecondThirdStage;
        [SerializeField] private int timesToRepeatSecondStage;
        [SerializeField] private Transform beamColliderTransform;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public IEnumerator StartAnimation()
        {
            WaitForSeconds firstStageDelay = new WaitForSeconds(1f / framesPerSecondFirstStage);
            WaitForSeconds secondStageDelay = new WaitForSeconds(1f / framesPerSecondSecondStage);
            WaitForSeconds thirdStageDelay = new WaitForSeconds(1f / framesPerSecondThirdStage);

            float durationFirstStage = (1f / framesPerSecondFirstStage) * firstStageSprites.Length;
            float durationThirdStage = (1f / framesPerSecondThirdStage) * thirdStageSprites.Length;

            beamColliderTransform.DOScaleY(1, durationFirstStage);

            foreach (Sprite sprite in firstStageSprites)
            {
                _spriteRenderer.sprite = sprite;
                yield return firstStageDelay;
            }

            for (int i = 0; i < timesToRepeatSecondStage; i++)
            {
                foreach (Sprite sprite in secondStageSprites)
                {
                    _spriteRenderer.sprite = sprite;
                    yield return secondStageDelay;
                }
            }

            beamColliderTransform.DOScaleY(0, durationThirdStage);

            foreach (Sprite sprite in thirdStageSprites)
            {
                _spriteRenderer.sprite = sprite;
                yield return thirdStageDelay;
            }
        }
    }
}