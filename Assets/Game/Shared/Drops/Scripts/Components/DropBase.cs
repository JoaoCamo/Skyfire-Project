using UnityEngine;

namespace Game.Drop
{
    public class DropBase : MonoBehaviour
    {
        [SerializeField] private DropType dropType;
        private bool _canGoToPlayer = false;
        protected const int PLAYER_LAYER = 6;
        protected const int PLAYER_COLLECT_LAYER = 11;

        public bool CanGoToPlayer => _canGoToPlayer;
        public DropType DropType => dropType;

        protected virtual void OnTriggerEnter2D(Collider2D collision) { }

        protected void GoToPlayer()
        {
            if (_canGoToPlayer)
                return;

            _canGoToPlayer = true;
        }

        protected virtual void OnCollect()
        {
            _canGoToPlayer = false;
        }
    }
}