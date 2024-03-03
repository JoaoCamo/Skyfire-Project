using System.Linq;
using Game.Player;
using UnityEngine;

namespace Game.Drop
{
    public class DropMovementController : MonoBehaviour
    {
        private DropManager _dropManager;

        private void Awake()
        {
            _dropManager = GetComponent<DropManager>();
        }

        private void FixedUpdate()
        {
            foreach (DropBase dropBase in _dropManager.DropBases.Where(dropBase => dropBase.CanGoToPlayer && dropBase.gameObject.activeSelf))
            {
                dropBase.GoToPlayer(PlayerMovement.PlayerTransform.position);
            }
        }
    }
}