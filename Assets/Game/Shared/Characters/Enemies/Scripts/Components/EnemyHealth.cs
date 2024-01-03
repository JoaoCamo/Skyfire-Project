using UnityEngine;

namespace Game.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private GameObject[] possibleDrops;

        private const int PLAYER_PROJECTILE_LAYER = 7;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PLAYER_PROJECTILE_LAYER) return;
            
            ReceiveDamage();
            other.gameObject.SetActive(false);
        }

        private void ReceiveDamage()
        {
            if (--health > 0) return;
            
            //DropItems();
            Destroy(gameObject);
        }

        private void DropItems()
        {
            //foreach (DropBase drop in possibleDrops)
            //{
            //    
            //}
        }
    }
}