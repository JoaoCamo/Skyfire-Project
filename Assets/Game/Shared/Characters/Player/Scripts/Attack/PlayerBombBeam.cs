using System.Collections;
using UnityEngine;
using Game.Animation;

namespace Game.Player
{
    public class PlayerBombBeam : MonoBehaviour
    {
        [SerializeField] private BeamAnimation beamAnimation;
        [SerializeField] private Transform hitBoxTransform;

        private readonly WaitForFixedUpdate _delay = new WaitForFixedUpdate();

        private bool _canFollow = false;
        private Coroutine _followCoroutine;

        public IEnumerator StartBeam()
        {
            _canFollow = true;

            _followCoroutine = StartCoroutine(FollowParent());

            yield return StartCoroutine(beamAnimation.StartAnimation());

            _canFollow = false;

            if(_followCoroutine != null)
            {
                StopCoroutine( _followCoroutine );
                _followCoroutine = null;
            }

            Destroy(gameObject);
        }

        private IEnumerator FollowParent()
        {
            while (_canFollow)
            {
                hitBoxTransform.localPosition = transform.localPosition;
                yield return _delay;
            }
        }
    }
}