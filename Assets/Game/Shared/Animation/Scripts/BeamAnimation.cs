using System.Collections;
using UnityEngine;

public class BeamAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] firstStageSprites;
    [SerializeField] private Sprite[] secondStageSprites;
    [SerializeField] private Sprite[] thirdStageSprites;
    [SerializeField] private int timesToRepeatSecondStage;
    [SerializeField] private int framesPerSecond;

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

        for (int i = 0; i < timesToRepeatSecondStage; i++)
        {
            foreach (Sprite sprite in secondStageSprites)
            {
                _spriteRenderer.sprite = sprite;
                yield return _animationDelay;
            }
        }

        foreach (Sprite sprite in thirdStageSprites)
        {
            _spriteRenderer.sprite = sprite;
            yield return _animationDelay;
        }
    }
}
