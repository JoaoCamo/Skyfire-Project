using System.Collections;
using UnityEngine;

namespace Game.Enemy.Boss
{
    public class BossIndicator : MonoBehaviour
    {
        [SerializeField] private Transform indicatorTransform;

        private Transform _bossTransform;
        private Coroutine _followCoroutine;
        private bool _canFollow;

        private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        public void StartFollow(Transform bossTransform)
        {
            gameObject.SetActive(true);
            _bossTransform = bossTransform;

            _canFollow = false;

            if (_followCoroutine != null)
            {
                StopCoroutine(_followCoroutine);
                _followCoroutine = null;
            }

            _canFollow = true;

            _followCoroutine = StartCoroutine(Follow());
        }

        public void StopFollow()
        {
            _canFollow = false;

            if(_followCoroutine != null)
            {
                StopCoroutine(_followCoroutine);
                _followCoroutine = null;
            }

            _bossTransform = null;
            gameObject.SetActive(false);
        }

        private IEnumerator Follow()
        {
            while(_canFollow)
            {
                transform.localPosition = new Vector3(_bossTransform.position.x, -0.625f, 0);
                yield return _waitForFixedUpdate;
            }
        }
    }
}