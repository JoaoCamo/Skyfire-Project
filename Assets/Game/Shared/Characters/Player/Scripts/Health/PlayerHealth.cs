using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Static.Events;
using Game.Gameplay.Effects;
using Game.Gameplay.UI;
using Game.Audio;
using Game.Drop;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private int _health = 2;
        private bool _canTakeDamage = true;
        private PlayerAttackBase _playerAttack;
        private PlayerMovement _playerMovement;
        private readonly WaitForSeconds _invincibilityDelay = new WaitForSeconds(0.25f);
        private readonly WaitForSeconds _stopActionDelay = new WaitForSeconds(0.5f);

        private readonly int[] _pointsNeededForExtraLife = new int[] { 1000000, 2000000, 4000000, 600000, 10000000 };
        private int _extraLifeIndex = 0;

        private const int MAX_HEALTH_VALUE = 8;
        private const int ENEMY_BULLET_LAYER = 9;
        private const int ENEMY_LAYER = 8;

        public Action RequestInvincibility;
        public static Action RequestNewLife;
        public static Action<int> RequestCheckForExtraLife;

        private void Awake()
        {
            _playerAttack = GetComponent<PlayerAttackBase>();
            _playerMovement = GetComponent<PlayerMovement>();
            GameEvents.OnHealthValueChange?.Invoke(_health);
        }

        private void OnEnable()
        {
            RequestInvincibility += StartInvincibility;
            RequestNewLife += AddLife;
            RequestCheckForExtraLife += CheckForExtraLife;
            GameEvents.OnRetry += RetryReset;
        }

        private void OnDisable()
        {
            RequestInvincibility -= StartInvincibility;
            RequestNewLife -= AddLife;
            RequestCheckForExtraLife -= CheckForExtraLife;
            GameEvents.OnRetry -= RetryReset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == ENEMY_BULLET_LAYER)
                other.gameObject.SetActive(false);
            
            if (!_canTakeDamage) return;
            
            if (other.gameObject.layer != ENEMY_BULLET_LAYER && other.gameObject.layer != ENEMY_LAYER) return;

            StartInvincibility();
            StartCoroutine(StopMovement());
            SpecialEffectsManager.RequestBulletClearShockwave(transform.position, 1.5f);
            SpecialEffectsManager.RequestExplosion(transform.position);
            SoundEffectController.RequestSfx(SfxTypes.PlayerHit);

            if (--_health < 0)
                GameEvents.OnGameEndLose?.Invoke(); 
            else
            {
                PlayerAttackBase.RequestNewBomb(3, true);
                PlayerAttackBase.RequestPowerValueChange(-1.5f);
                GameEvents.OnHealthValueChange(_health);
            }
        }

        private void StartInvincibility()
        {
            StartCoroutine(StartInvincibilityCoroutine());
        }

        private IEnumerator StartInvincibilityCoroutine()
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
            _playerMovement.SpeedMultiplayer = 0;
            _playerAttack.CanShoot = false;
            _playerAttack.CanBomb = false;
            yield return _stopActionDelay;
            _playerMovement.SpeedMultiplayer = 1;
            _playerAttack.CanShoot = true;
            _playerAttack.CanBomb = true;
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
                GameEvents.OnHealthValueChange(_health);
                PopUpTextManager.RequestPopUpText(new Vector2(0,0.4f), "EXTEND!", Color.grey);
            }
        }

        private void CheckForExtraLife(int points)
        {
            if (_extraLifeIndex > _pointsNeededForExtraLife.Length)
                return;

            if(points >= _pointsNeededForExtraLife[_extraLifeIndex])
            {
                _extraLifeIndex++;
                AddLife();
            }
        }

        private void RetryReset()
        {
            Vector3 dropPositon = transform.position + new Vector3(0, 0.25f, 0);

            _health = 2;
            GameEvents.OnHealthValueChange(_health);
            PlayerAttackBase.RequestNewBomb(3, true);
            PlayerAttackBase.RequestPowerValueChange(-4);
            DropManager.RequestDrop(DropType.FullPower, dropPositon);
        }
    }
}