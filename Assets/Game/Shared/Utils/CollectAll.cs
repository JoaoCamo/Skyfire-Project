using UnityEngine;
using Game.Drop;

namespace Game.Utils
{
    public class CollectAll : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            DropManager.RequestCollectAll?.Invoke();
        }
    }
}