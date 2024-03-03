using UnityEngine;

namespace Game.Player
{
    public class PlayerBombCollider : MonoBehaviour
    {
        private const int ENEMY_PROJECTILE_LAYER = 9;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != ENEMY_PROJECTILE_LAYER) return;
            
            other.gameObject.SetActive(false);
        }
    }
}