using UnityEngine;
using Game.Danmaku;

namespace Game.Enemy.Boss
{
    public class BossAttackController : MonoBehaviour
    {
        [SerializeField] private BossAttackInfo[] bossAttackInfo;
        [SerializeField] private EnemyAttackReference attackReference;

        private BossMovementController _movementController;

        private EnemyAttackBase _attackBase;
        private Coroutine _attackCoroutine;

        private void Awake()
        {
            _movementController = GetComponent<BossMovementController>();
        }

        public void InitiliazeNextAttack(int attackIndex)
        {
            StopAttack();

            EnemyAttackPatterns attackPattern = bossAttackInfo[attackIndex].attackInfo.attackPattern;

            _attackBase = Instantiate(attackReference.enemyAttackPrefabs[(int)attackPattern], transform).GetComponent<EnemyAttackBase>();
            _attackBase.SetShot(bossAttackInfo[attackIndex].attackInfo);

            _attackCoroutine = StartCoroutine(_attackBase.Shoot());

            switch(bossAttackInfo[attackIndex].hasMovement)
            {
                case true:
                    _movementController.StartRandomMovement();
                    break;
                case false:
                    _movementController.ReturnToStartPosition();
                    break;
            }
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