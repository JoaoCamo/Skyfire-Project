using UnityEngine;
using Game.Player;
using Game.Gameplay.UI;
using Game.Static.Events;

namespace Game.Drop
{
    public class BombDrop : DropBase
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
            PlayerAttack.RequestNewBomb(1, false);
            GameEvents.OnPointsValueChange?.Invoke(dropPointsValue);
            PopUpTextManager.RequestPopUpText.Invoke(transform.position, dropPointsValue.ToString(), Color.grey);

            gameObject.SetActive(false);
        }
    }
}
