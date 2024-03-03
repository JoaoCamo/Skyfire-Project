using UnityEngine;
using Game.Drop;
using Game.Static.Events;

namespace Game.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private PossibleDrops[] possibleDrops;
        
        private int _health;
        private bool _hasDroppedItems = false;

        private EnemyAttackController _attack;
        private EnemyMovement _movement;
        private const int PLAYER_PROJECTILE_LAYER = 7;
        private const int PLAYER_BOMB_LAYER = 13;

        private void Awake()
        {
            _attack = GetComponent<EnemyAttackController>();
            _movement = GetComponent<EnemyMovement>();
        }

        private void OnDisable()
        {
            _attack.StopAttack();
            _movement.StopMovement();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PLAYER_PROJECTILE_LAYER && other.gameObject.layer != PLAYER_BOMB_LAYER) return;
            
            ReceiveDamage(other.gameObject.layer == PLAYER_PROJECTILE_LAYER ? 1 : 20);
            
            if(other.gameObject.layer == PLAYER_PROJECTILE_LAYER)
                other.gameObject.SetActive(false);
        }

        private void ReceiveDamage(int damage)
        {
            _health -= damage;
            if (_health > 0) return;
            
            GameEvents.OnPointsValueChange?.Invoke(10);
            DropItems();
            gameObject.SetActive(false);
        }

        private void DropItems()
        {
            if (_hasDroppedItems)
                    return;

            _hasDroppedItems = true;
            
            foreach (PossibleDrops possibleDrop in possibleDrops)
            {
                if (!(Random.value <= possibleDrop.dropChance)) continue;
                
                var originPosition = transform.position;
                float xPosition = originPosition.x + Random.Range(-0.1f, 0.1f);
                float yPosition = originPosition.y + Random.Range(-0.1f, 0.1f);
                DropManager.RequestDrop?.Invoke(possibleDrop.dropType, new Vector3(xPosition,yPosition));
            }
        }

        public void ResetHealth(int newHealthValue)
        {
            _health = newHealthValue;
            _hasDroppedItems = false;
        }
    }

    [System.Serializable]
    public struct PossibleDrops
    {
        public DropType dropType;
        public float dropChance;
    }
}