using UnityEngine;
using Game.Player;
using Game.Gameplay.UI;
using Game.Static.Events;

namespace Game.Drop
{
    public class LifeDrop : DropBase
    {
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
            PlayerHealth.RequestNewLife();
            GameEvents.OnPointsValueChange(dropPointsValue);
            PopUpTextManager.RequestPopUpText(transform.position, dropPointsValue.ToString(), 15, Color.grey);

            gameObject.SetActive(false);
        }
    }
}
