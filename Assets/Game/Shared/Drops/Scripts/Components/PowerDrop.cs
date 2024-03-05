using UnityEngine;
using Game.Player;
using Game.Gameplay.UI;
using Game.Static.Events;

namespace Game.Drop
{
    public class PowerDrop : DropBase
    {
        [SerializeField] private float powerValue;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.layer)
            {
                case PLAYER_COLLECT_LAYER:
                    _canGoToPlayer = true;
                    break;
                case PLAYER_LAYER:
                    OnCollect();
                    break;
            }
        }

        protected override void OnCollect()
        {
            base.OnCollect();
            PlayerAttack.RequestPowerValueChange?.Invoke(powerValue);
            GameEvents.OnPointsValueChange?.Invoke(dropPointsValue);
            PopUpTextManager.RequestPopUpText.Invoke(transform.position, dropPointsValue.ToString(), Color.grey);

            gameObject.SetActive(false);
        }
    }
}