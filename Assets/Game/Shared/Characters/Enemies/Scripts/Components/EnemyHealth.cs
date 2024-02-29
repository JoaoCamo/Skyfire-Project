using UnityEngine;

namespace Game.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private PossibleDrops[] possibleDrops;

        private int _currentHealth;

        private EnemyAttackController _attack;
        private EnemyMovement _movement;
        private const int PLAYER_PROJECTILE_LAYER = 7;

        private void Awake()
        {
            _attack = GetComponent<EnemyAttackController>();
            _movement = GetComponent<EnemyMovement>();

            ResetHealth();
        }

        private void OnDisable()
        {
            _attack.StopAttack();
            _movement.StopMovement();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PLAYER_PROJECTILE_LAYER) return;
            
            ReceiveDamage();
            other.gameObject.SetActive(false);
        }

        private void ReceiveDamage()
        {
            if (--_currentHealth > 0) return;

            DropItems();
            gameObject.SetActive(false);
        }

        private void DropItems()
        {
            //foreach (DropBase drop in possibleDrops)
            //{
            //    
            //}
        }

        public void ResetHealth()
        {
            _currentHealth = health;
        }
    }

    [System.Serializable]
    public struct PossibleDrops
    {
        //public DropType dropType;
        public float dropChance;
    }
}