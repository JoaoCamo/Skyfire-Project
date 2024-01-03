using System;
using UnityEngine;
using Game.Projectiles;

namespace Game.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        private EnemyAttackPatterns _enemyAttackPattern;
        private ProjectileType _projectileType;
        private bool _isAimed = false;
        private bool _isLoop = false;
        private int _timesToShoot;
        private float _speed;
        private float _delayBetweenShots;

        private Coroutine _attackCoroutine;

        public void StartShot()
        {
            switch (_enemyAttackPattern)
            {
                case EnemyAttackPatterns.SingleShot:
                    break;
                case EnemyAttackPatterns.RoundShot:
                    break;
                case EnemyAttackPatterns.TriangleShot:
                    break;
                case EnemyAttackPatterns.ProgressiveRoundShot:
                    break;
                default:
                    break;
            }
        }

        private void SetAttack(EnemyAttackPatterns enemyAttackPattern, ProjectileType projectileType, bool isAimed, bool isLoop, float speed, float delayBetweenShots)
        {
            _enemyAttackPattern = enemyAttackPattern;
            _projectileType = projectileType;
            _isAimed = isAimed;
            _isLoop = isLoop;
            _speed = speed;
            _delayBetweenShots = delayBetweenShots;
        }
        
        private void SetAttack(EnemyAttackPatterns enemyAttackPattern, ProjectileType projectileType, bool isAimed, int timesToShoot, float speed, float delayBetweenShots)
        {
            _enemyAttackPattern = enemyAttackPattern;
            _projectileType = projectileType;
            _isAimed = isAimed;
            _timesToShoot = timesToShoot;
            _speed = speed;
            _delayBetweenShots = delayBetweenShots;
        }
    }
}
