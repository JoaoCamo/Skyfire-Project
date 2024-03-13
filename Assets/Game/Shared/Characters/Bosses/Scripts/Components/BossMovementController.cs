using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Enemy.Boss
{
    public class BossMovementController : MonoBehaviour
    {
        private readonly Vector2 _initialPosition = new Vector2 (-0.375f, 0.45f);
        private readonly WaitForSeconds _randomMovementDelay = new WaitForSeconds(3);
        private readonly WaitForSeconds _returnToInitialPositionDelay = new WaitForSeconds(1);
        private readonly WaitForSeconds _movementDurationDelay = new WaitForSeconds(2);
            
        private const float X_MIN_POSITION = -1.15f;
        private const float X_MAX_POSITION = 0.4f;
        private const float Y_MIN_POSITION = 0;
        private const float Y_MAX_POSITION = 0.8f;
        
        private bool _canMove = false;

        private Coroutine _randomMovementCoroutine = null;

        private void Start()
        {
            ReturnToStartPosition();
        }

        public void ReturnToStartPosition()
        {
            StopRandomMovement();
            transform.DOMove(_initialPosition, 1).SetEase(Ease.Linear);
        }

        public IEnumerator StartRandomMovement()
        {
            StopRandomMovement();
            transform.DOMove(_initialPosition, 1).SetEase(Ease.Linear);

            yield return _returnToInitialPositionDelay;
            
            _canMove = true;
            _randomMovementCoroutine = StartCoroutine(RandomMovement());
        }

        private IEnumerator RandomMovement()
        {
            while (_canMove)
            {
                yield return _randomMovementDelay;

                float xPosition = Random.Range(X_MIN_POSITION, X_MAX_POSITION);
                float yPosition = Random.Range(Y_MIN_POSITION, Y_MAX_POSITION);

                transform.DOMove(new Vector3(xPosition, yPosition), 2);

                yield return _movementDurationDelay;
            }
        }

        private void StopRandomMovement()
        {
            if (_randomMovementCoroutine != null)
            {
                _canMove = false;
                StopCoroutine(_randomMovementCoroutine);
                _randomMovementCoroutine = null;
            }
            
            transform.DOKill();
        }
    }
}