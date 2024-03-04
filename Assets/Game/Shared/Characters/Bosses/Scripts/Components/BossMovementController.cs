using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Enemy.Boss
{
    public class BossMovementController : MonoBehaviour
    {
        private readonly Vector2 _initialPosition = new Vector2 (-0.375f, 0.45f);
        private readonly WaitForSeconds _randomMovementDelay = new WaitForSeconds(3);

        private Rigidbody2D _rigidBody;

        private float _xDirection;
        private float _yDirection;
        private bool _canMove = false;

        private Coroutine _randomMovementCoroutine;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            ReturnToStartPosition();
        }

        private void FixedUpdate()
        {
            _rigidBody.velocity = new Vector2(_xDirection, _yDirection) * 0.25f;
        }

        public void ReturnToStartPosition()
        {
            StopRandomMovement();
            transform.DOMove(_initialPosition, 1).SetEase(Ease.Linear);
        }

        public void StartRandomMovement()
        {
            StopRandomMovement();
            _canMove = true;
            _randomMovementCoroutine = StartCoroutine(RandomMovement());
        }

        private IEnumerator RandomMovement()
        {
            float movementDuration;

            while (_canMove)
            {
                yield return _randomMovementDelay;

                _xDirection = Random.Range(-1f, 1f);
                _yDirection = Random.Range(-1f, 1f);

                movementDuration = Random.Range(0f, 2f);
                
                yield return new WaitForSeconds(movementDuration);

                _xDirection = 0;
                _yDirection = 0;
            }
        }

        private void StopRandomMovement()
        {
            _canMove = false;

            if(_randomMovementCoroutine != null)
            {
                StopCoroutine( _randomMovementCoroutine);
                _randomMovementCoroutine = null;
            }
        }
    }
}