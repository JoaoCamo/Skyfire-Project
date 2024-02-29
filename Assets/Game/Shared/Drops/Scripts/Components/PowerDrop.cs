using Game.Player;
using UnityEngine;

namespace Game.Drop
{
    public class PowerDrop : DropBase
    {
        [SerializeField] private float powerValue;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PLAYER_COLLECT_LAYER)
                GoToPlayer();
            else if (collision.gameObject.layer == PLAYER_LAYER)
                OnCollect();
        }

        protected override void OnCollect()
        {
            base.OnCollect();
            PlayerAttack.RequestPowerValueChange?.Invoke(powerValue);

            gameObject.SetActive(false);
        }
    }
}