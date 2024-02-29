using UnityEngine;

namespace Game.Drop
{
    public class DropBase : MonoBehaviour
    {
        [SerializeField] private DropType dropType;
        private bool _canGoToPlayer = false;

        public bool CanGoToPlayer => _canGoToPlayer;
        public DropType DropType => dropType;

        protected virtual void OnTriggerEnter2D(Collider2D collision) { }

        protected void GoToPlayer()
        {
            _canGoToPlayer = true;
        }

        protected virtual void OnCollect()
        {
            _canGoToPlayer = false;
        }
    }
}