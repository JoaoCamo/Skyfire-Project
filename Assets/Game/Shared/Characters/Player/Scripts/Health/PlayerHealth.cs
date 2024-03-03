using System.Collections;
using DG.Tweening;
using UnityEngine;
using Game.Static.Events;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject playerHitShockwavePrefab;
        
        private int _health = 2;
        private bool _canTakeDamage = true;
        private readonly WaitForSeconds _invincibilityDelay = new WaitForSeconds(0.25f);
        private readonly WaitForSeconds _shockwaveFadeDelay = new WaitForSeconds(0.5f);

        private const int MAX_HEALTH_VALUE = 8;
        private const int ENEMY_BULLET_LAYER = 9;
        private const int ENEMY_LAYER = 8;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canTakeDamage) return;
            
            if (other.gameObject.layer != ENEMY_BULLET_LAYER && other.gameObject.layer != ENEMY_LAYER) return;

            if (--_health < 0) GameEvents.OnGameEnd?.Invoke(false);
            else
            {
                GameEvents.OnHealthValueChange?.Invoke(_health);
                StartCoroutine(StartInvincibility());
                StartCoroutine(StartShockwave());
            }
        }

        private IEnumerator StartInvincibility()
        {
            _canTakeDamage = false;

            for (int i = 0; i < 12; i++)
            {
                Color originalColor = spriteRenderer.color;
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, i % 2 == 0 ? 0.25f : 1);
                spriteRenderer.DOColor(newColor, 0.25f);
                
                yield return _invincibilityDelay;
            }
            _canTakeDamage = true;
        }

        private IEnumerator StartShockwave()
        {
            SpriteRenderer shockwaveSpriteRenderer = Instantiate(playerHitShockwavePrefab, transform).GetComponent<SpriteRenderer>();
            shockwaveSpriteRenderer.transform.DOScale(1, 1);
            
            yield return _shockwaveFadeDelay;
            
            Color originalColor = shockwaveSpriteRenderer.color;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            
            shockwaveSpriteRenderer.DOColor(newColor, 0.25f);

            yield return _shockwaveFadeDelay;
            
            Destroy(shockwaveSpriteRenderer.gameObject);
        }

        private void AddLife()
        {
            if (_health + 1 > MAX_HEALTH_VALUE) return;

            _health++;
            GameEvents.OnHealthValueChange?.Invoke(_health);
        }
    }
}