using System.Collections;
using UnityEngine;
using Game.Animation;

namespace Game.Player
{
    public class PlayerBombShield : MonoBehaviour
    {
        [SerializeField] private ShieldAnimation beamAnimation;

        public IEnumerator StartShield()
        {
            yield return StartCoroutine(beamAnimation.StartAnimation());

            Destroy(gameObject);
        }
    }
}