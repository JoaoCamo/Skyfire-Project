using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        private readonly float[] xSpawnPosition = {-1.425f, -0.825f, -0.35f, 0.125f, 0.7f};
        private readonly float[] ySpawnPosition = {1.1f, 0.8f, 0.45f, 0.2f};

        private Vector2 movementDirection = Vector2.zero;
        private float speed = 0;

        private Coroutine _movementChangeCoroutine;

        public void UpdatePosition()
        {
            transform.Translate(movementDirection * speed * Time.deltaTime);
        }

        public void SetPosition(EnemyInitialPosition enemyInitialPosition)
        {
            transform.position = new Vector2(xSpawnPosition[(int)enemyInitialPosition.enemyInitialPositionX], ySpawnPosition[(int)enemyInitialPosition.enemyInitialPositionY]);
        }

        public void SetMovement(EnemyMovementInfo enemyMovementInfo)
        {
            this.movementDirection = enemyMovementInfo.movementDirection;
            this.speed = enemyMovementInfo.speed;

            if (enemyMovementInfo.movementChangeInfo.Length > 0)
            {
                if (_movementChangeCoroutine != null)
                {
                    StopCoroutine(_movementChangeCoroutine);
                    _movementChangeCoroutine = null;
                }

                _movementChangeCoroutine = StartCoroutine(MovementChangeCoroutine(enemyMovementInfo));
            }
        }

        public void StopMovement()
        {
            if (_movementChangeCoroutine != null)
            {
                StopCoroutine(_movementChangeCoroutine);
                _movementChangeCoroutine = null;
            }

            movementDirection = Vector2.zero;
            speed = 0;
        }

        private IEnumerator MovementChangeCoroutine(EnemyMovementInfo enemyMovementInfo)
        {
            foreach (MovementChangeInfo movementChangeInfo in enemyMovementInfo.movementChangeInfo)
            {
                yield return new WaitForSeconds(movementChangeInfo.changeDelay);
                movementDirection = movementChangeInfo.movementDirection;
            }
        }
    }
}