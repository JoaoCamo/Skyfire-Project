using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private readonly float[] _xSpawnPosition = {-1.425f, -0.825f, -0.35f, 0.125f, 0.7f};
        private readonly float[] _ySpawnPosition = {1.1f, 0.8f, 0.45f, 0.2f};

        private Rigidbody2D _rigidbody2D;
        private float _xDirection = 0;
        private float _yDirection = 0;
        private float _speed = 0;

        private Coroutine _movementChangeCoroutine;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            StopMovement();
        }

        public void UpdatePosition()
        {
            _rigidbody2D.velocity = new Vector2(_xDirection, _yDirection) * _speed;
        }

        public void SetPosition(EnemyInitialPosition enemyInitialPosition)
        {
            transform.position = new Vector2(_xSpawnPosition[(int)enemyInitialPosition.enemyInitialPositionX], _ySpawnPosition[(int)enemyInitialPosition.enemyInitialPositionY]);
        }

        public void SetMovement(EnemyMovementInfo enemyMovementInfo)
        {
            this._xDirection = enemyMovementInfo.movementDirection.x;
            this._yDirection = enemyMovementInfo.movementDirection.y;
            this._speed = enemyMovementInfo.speed;

            if (_movementChangeCoroutine != null)
            {
                StopCoroutine(_movementChangeCoroutine);
                _movementChangeCoroutine = null;
            }

            _movementChangeCoroutine = StartCoroutine(MovementChangeCoroutine(enemyMovementInfo));
        }

        private void StopMovement()
        {
            if (_movementChangeCoroutine != null)
            {
                StopCoroutine(_movementChangeCoroutine);
                _movementChangeCoroutine = null;
            }

            _xDirection = 0;
            _yDirection = 0;
            _speed = 0;
        }

        private IEnumerator MovementChangeCoroutine(EnemyMovementInfo enemyMovementInfo)
        {
            foreach (MovementChangeInfo movementChangeInfo in enemyMovementInfo.movementChangeInfo)
            {
                yield return new WaitForSeconds(movementChangeInfo.changeDelay);
                DOTween.To(() => _xDirection, x => _xDirection = x, movementChangeInfo.movementDirection.x, 0.75f).SetEase(Ease.Linear);
                DOTween.To(() => _yDirection, x => _yDirection = x, movementChangeInfo.movementDirection.y, 0.75f).SetEase(Ease.Linear);
            }
        }
    }
}