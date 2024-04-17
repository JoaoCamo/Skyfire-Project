using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Static.Events;
using Game.Animation;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject shockwavePrefab;
        
        private int _health = 2;
        private bool _canTakeDamage = true;
        private PlayerAttackBase _playerAttack;
        private PlayerMovement _playerMovement;
        private readonly WaitForSeconds _invincibilityDelay = new WaitForSeconds(0.25f);
        private readonly WaitForSeconds _stopActionDelay = new WaitForSeconds(0.5f);

        private const int MAX_HEALTH_VALUE = 8;
        private const int ENEMY_BULLET_LAYER = 9;
        private const int ENEMY_LAYER = 8;

        public Action RequestInvincibility;
        public static Action RequestNewLife;

        private void Awake()
        {
            _playerAttack = GetComponent<PlayerAttackBase>();
            _playerMovement = GetComponent<PlayerMovement>();
            GameEvents.OnHealthValueChange?.Invoke(_health);

            RequestInvincibility += () => { StartCoroutine(StartInvincibility()); };
        }

        private void OnEnable()
        {
            RequestNewLife += AddLife;
            GameEvents.OnRetry += RetryReset;
        }

        private void OnDisable()
        {
            RequestInvincibility -= 
            RequestNewLife -= AddLife;
            GameEvents.OnRetry -= RetryReset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == ENEMY_BULLET_LAYER)
                other.gameObject.SetActive(false);
            
            if (!_canTakeDamage) return;
            
            if (other.gameObject.layer != ENEMY_BULLET_LAYER && other.gameObject.layer != ENEMY_LAYER) return;

            StartCoroutine(StartInvincibility());
            StartCoroutine(StopMovement());
            StartShockWave();

            if (--_health < 0)
                GameEvents.OnGameEndLose?.Invoke(); 
            else
            {
                PlayerAttackBase.RequestNewBomb?.Invoke(3, true);
                PlayerAttackBase.RequestPowerValueChange?.Invoke(-1.5f);
                GameEvents.OnHealthValueChange?.Invoke(_health);
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

        private IEnumerator StopMovement()
        {
            _playerAttack.CanShoot = false;
            _playerMovement.SpeedMultiplayer = 0;
            yield return _stopActionDelay;
            _playerMovement.SpeedMultiplayer = 1;
            _playerAttack.CanShoot = true;
        }

        private void StartShockWave()
        {
            Instantiate(shockwavePrefab, transform.position, Quaternion.identity).GetComponent<BulletClearShockwaveAnimation>().StartShockwave(0.75f);
        }

        private void AddLife()
        {
            if (_health + 1 > MAX_HEALTH_VALUE)
            {
                PlayerAttackBase.RequestNewBomb(1, false);
            }
            else
            {
                _health++;
                GameEvents.OnHealthValueChange?.Invoke(_health);
            }
        }

        private void RetryReset()
        {
            _health = 2;
            PlayerAttackBase.RequestNewBomb?.Invoke(3, true);
            GameEvents.OnHealthValueChange?.Invoke(_health);
            PlayerAttackBase.RequestPowerValueChange?.Invoke(4);
        }
    }
}