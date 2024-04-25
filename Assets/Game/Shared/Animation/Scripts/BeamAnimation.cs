using System.Collections;
using UnityEngine;

public class BeamAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] firstStageSprites;
    [SerializeField] private Sprite[] secondStageSprites;
    [SerializeField] private Sprite[] thirdStageSprites;
    [SerializeField] private int timesToRepeatSecondStage;
    [SerializeField] private int framesPerSecond;
    [SerializeField] private CircleCollider2D beamCollider;

    private WaitForSeconds _animationDelay;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        float timeDelay = 1f / framesPerSecond;

        _animationDelay = new WaitForSeconds(timeDelay);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator StartAnimation()
    {
        foreach (Sprite sprite in firstStageSprites)
        {
            _spriteRenderer.sprite = sprite;
            yield return _animationDelay;
        }

        beamCollider.enabled = true;

        for (int i = 0; i < timesToRepeatSecondStage; i++)
        {
            foreach (Sprite sprite in secondStageSprites)
            {
                _spriteRenderer.sprite = sprite;
                yield return _animationDelay;
            }
        }

        beamCollider.enabled = false;

        foreach (Sprite sprite in thirdStageSprites)
        {
            _spriteRenderer.sprite = sprite;
            yield return _animationDelay;
        }
    }
}
