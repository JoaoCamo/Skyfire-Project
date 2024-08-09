using System.Collections;
using UnityEngine;
using Game.Danmaku;
using Game.Projectiles;

namespace Game.Enemy.Boss
{
    public class BossAttackController : MonoBehaviour
    {
        [SerializeField] private EnemyAttackReference attackReference;

        private EnemyProjectileManager _enemyProjectileManager;

        private BossAttackInfo[] _bossAttackInfo;
        private BossMovementController _movementController;

        private DanmakuBase _attackBase;
        private Coroutine _attackCoroutine;

        public EnemyProjectileManager EnemyProjectileManager { set => _enemyProjectileManager = value; }
        public BossAttackInfo[] BossAttackInfo { set => _bossAttackInfo = value; }

        private void Awake()
        {
            _movementController = GetComponent<BossMovementController>();
        }

        public IEnumerator InitializeNextAttack(int attackIndex) 
        {
            StopAttack();

            _movementController.ReturnToCentralPosition();

            EnemyAttackPatterns attackPattern = _bossAttackInfo[attackIndex].attackInfo.attackPattern;

            _attackBase = Instantiate(attackReference.enemyAttackPrefabs[(int)attackPattern], transform).GetComponent<DanmakuBase>();
            _attackBase.SetShot(_bossAttackInfo[attackIndex].attackInfo, _enemyProjectileManager);

            yield return new WaitForSeconds(_bossAttackInfo[attackIndex].attackInfo.shotStartDelay);

            if (_bossAttackInfo[attackIndex].hasMovement)
                StartCoroutine(_movementController.StartRandomMovement(_bossAttackInfo[attackIndex].movementDelay));

            _attackCoroutine = StartCoroutine(_attackBase.Shoot());

        }

        private void StopAttack()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            if (_attackBase != null)
            {
                Destroy(_attackBase.gameObject);
                _attackBase = null;
            }
        }
    }
}