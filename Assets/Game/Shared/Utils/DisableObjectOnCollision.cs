using UnityEngine;

namespace Game.Utils
{
    public class DisableObjectOnCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.gameObject.SetActive(false);
        }
    }
}