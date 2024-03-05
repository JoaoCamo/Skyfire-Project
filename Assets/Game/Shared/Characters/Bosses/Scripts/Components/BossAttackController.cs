using System.Collections;
using UnityEngine;
using Game.Danmaku;

namespace Game.Enemy.Boss
{
    public class BossAttackController : MonoBehaviour
    {
        [SerializeField] private EnemyAttackReference attackReference;

        private BossAttackInfo[] bossAttackInfo;
        private BossMovementController _movementController;

        private EnemyAttackBase _attackBase;
        private Coroutine _attackCoroutine;

        public BossAttackInfo[] BossAttackInfo { get => bossAttackInfo; set => bossAttackInfo = value; }

        private void Awake()
        {
            _movementController = GetComponent<BossMovementController>();
        }

        public IEnumerator InitializeNextAttack(int attackIndex) 
        {
            StopAttack();

            switch(bossAttackInfo[attackIndex].hasMovement)
            {
                case true:
                    _movementController.StartRandomMovement();
                    break;
                case false:
                    _movementController.ReturnToStartPosition();
                    break;
            }

            EnemyAttackPatterns attackPattern = bossAttackInfo[attackIndex].attackInfo.attackPattern;

            _attackBase = Instantiate(attackReference.enemyAttackPrefabs[(int)attackPattern], transform).GetComponent<EnemyAttackBase>();
            _attackBase.SetShot(bossAttackInfo[attackIndex].attackInfo);

            yield return new WaitForSeconds(bossAttackInfo[attackIndex].attackInfo.shotStartDelay);

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