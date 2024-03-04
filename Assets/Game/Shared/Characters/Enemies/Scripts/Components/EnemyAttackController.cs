using System;
using System.Collections;
using UnityEngine;
using Game.Danmaku;

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

        public void SetAttack(EnemyAttackInfo attackInfo)
        {
            _attackBase = Instantiate(attackReference.GetAttack(attackInfo.attackPattern), transform).GetComponent<EnemyAttackBase>();
            _attackBase.SetShot(attackInfo);

            StartCoroutine(StartAttack(attackInfo.shotStartDelay));
        }

        private IEnumerator StartAttack(float delay)
        {
            if(_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

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