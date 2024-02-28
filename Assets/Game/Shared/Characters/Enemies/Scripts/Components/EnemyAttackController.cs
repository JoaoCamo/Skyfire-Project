using UnityEngine;
using Game.Danmaku;

namespace Game.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private EnemyAttackReference attackReference;
        private EnemyAttackBase _attackBase;

        private Coroutine _attackCoroutine;
        
        public void SetAttack(EnemyAttackInfo attackInfo)
        {
            _attackBase = Instantiate(attackReference.GetAttack(attackInfo.attackPattern), transform).GetComponent<EnemyAttackBase>();
            _attackBase.SetShot(attackInfo);

            StartAttack();
        }

        public void StartAttack()
        {
            if(_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            _attackCoroutine = StartCoroutine(_attackBase.Shoot());
        }

        public void StopAttack()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            Destroy(_attackBase.gameObject);
            _attackBase = null;
        }
    }
}