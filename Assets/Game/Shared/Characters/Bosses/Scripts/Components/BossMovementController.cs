using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Enemy.Boss
{
    public class BossMovementController : MonoBehaviour
    {
        private readonly Vector2 _initialPosition = new Vector2(0, 1.1f);
        private readonly Vector2 _centralPosition = new Vector2 (0, 0.6f);
        private readonly WaitForSeconds _returnToCentralPositionDelay = new WaitForSeconds(1);
        private readonly WaitForSeconds _movementDurationDelay = new WaitForSeconds(2);
            
        private const float X_MIN_POSITION = -0.5f;
        private const float X_MAX_POSITION = 0.5f;
        private const float Y_MIN_POSITION = 0.25f;
        private const float Y_MAX_POSITION = 0.8f;
        
        private bool _canMove = false;

        private Coroutine _randomMovementCoroutine = null;

        private void Awake()
        {
            transform.position = _initialPosition;
        }

        private void Start()
        {
            ReturnToCentralPosition();
        }

        public void ReturnToCentralPosition()
        {
            StopRandomMovement();
            transform.DOMove(_centralPosition, 1).SetEase(Ease.Linear);
        }

        public IEnumerator StartRandomMovement(float movementDelay)
        {
            StopRandomMovement();
            transform.DOMove(_centralPosition, 1).SetEase(Ease.Linear);

            yield return _returnToCentralPositionDelay;
            
            _canMove = true;
            _randomMovementCoroutine = StartCoroutine(RandomMovement(movementDelay));
        }

        private IEnumerator RandomMovement(float movementDelay)
        {
            WaitForSeconds randomMovementDelay = new WaitForSeconds(movementDelay);

            while (_canMove)
            {
                yield return randomMovementDelay;

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