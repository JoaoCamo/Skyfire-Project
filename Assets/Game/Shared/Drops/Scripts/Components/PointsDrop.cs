using UnityEngine;
using Game.Static.Events;

namespace Game.Drop
{
    public class PointsDrop : DropBase
    {
        private const float MAX_VALUE_HEIGHT = 0.4f;
        private const float MIN_VALUE_HEIGHT = -0.65f;
        
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
            GameEvents.OnPointsValueChange?.Invoke((int)(dropPointsValue * GetDropValueMultiplayer()));
            gameObject.SetActive(false);
        }

        private float GetDropValueMultiplayer()
        {
            switch (transform.position.y)
            {
                case > MAX_VALUE_HEIGHT:
                    return 1;
                case < MIN_VALUE_HEIGHT:
                    return 0.3f;
            }
            
            float normalizedValue = Mathf.Clamp01((transform.position.y - MIN_VALUE_HEIGHT) / (MAX_VALUE_HEIGHT - MIN_VALUE_HEIGHT));
            float valueMultiplayer = 0.3f + normalizedValue * 0.7f;

            return valueMultiplayer;
        }
    }
}