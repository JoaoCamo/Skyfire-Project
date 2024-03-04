using UnityEngine;

namespace Game.Drop
{
    public class DropBase : MonoBehaviour
    {
        [SerializeField] private DropType dropType;
        [SerializeField] protected int dropPointsValue;
        private Rigidbody2D _rigidbody2D;
        protected bool _canGoToPlayer = false;
        protected const int PLAYER_LAYER = 6;
        protected const int PLAYER_COLLECT_LAYER = 11;

        public DropType DropType => dropType;
        public bool CanGoToPlayer { get => _canGoToPlayer; set => _canGoToPlayer = value; }

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) { }

        protected virtual void OnCollect()
        {
            _canGoToPlayer = false;
        }
        
        public void GoToPlayer(Vector3 playerPosition)
        {
            Vector2 direction = (playerPosition - transform.position).normalized;
            _rigidbody2D.velocity = direction * 1.5f;
        }
    }
}