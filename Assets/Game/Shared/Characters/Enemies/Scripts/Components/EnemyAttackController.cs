using System.Collections;
using UnityEngine;
using Game.Danmaku;
using Game.Projectiles;

namespace Game.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private EnemyAttackReference attackReference;
        private EnemyAttackBase _attackBase;

        private Coroutine _attackCoroutine;

        private void OnDisable()
        {
            StopAttack();
        }

        public void SetAttack(EnemyAttackInfo attackInfo, EnemyProjectileManager enemyProjectileManager)
        {
            StopAttack();

            _attackBase = Instantiate(attackReference.GetAttack(attackInfo.attackPattern), transform).GetComponent<EnemyAttackBase>();
            _attackBase.SetShot(attackInfo, enemyProjectileManager);

            StartCoroutine(StartAttack(attackInfo.shotStartDelay));
        }

        private IEnumerator StartAttack(float delay)
        {
            yield return new WaitForSeconds(delay);

            _attackCoroutine = StartCoroutine(_attackBase.Shoot());
        }

        private void StopAttack()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            if (_attackBase == null) return;
            
            Destroy(_attackBase.gameObject);
            _attackBase = null;
        }
    }
}