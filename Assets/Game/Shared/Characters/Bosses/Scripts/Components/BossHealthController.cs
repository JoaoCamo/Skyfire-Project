using System.Collections;
using UnityEngine;
using Game.Drop;
using Game.Stage;
using Game.Gameplay.UI;
using Game.Audio;
using Game.Gameplay.Effects;
using Game.Static.Events;

namespace Game.Enemy.Boss
{
    public class BossHealthController : MonoBehaviour
    {
        private BossHealthInfo[] _bossHealthInfo;
        
        private BossAttackController _attackController;
        private BossMovementController _movementController;

        private BossHealthInfo _currentHealthInfo;
        private int _bossHealthBars = 0;
        private int _currentHealthBar = 0;
        private int _currentHealth = 0;
        private bool _hasDroppedItems = false;
        private bool _canTakeDamage = false;
        private bool _destroyWhenDefeat;

        private readonly WaitForSeconds _canTakeDamageDelay = new WaitForSeconds(1.25f);
        private const int PLAYER_PROJECTILE_LAYER = 7;
        private const int PLAYER_BOMB_LAYER = 13;

        private void Awake()
        {
            _attackController = GetComponent<BossAttackController>();
            _movementController = GetComponent<BossMovementController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PLAYER_PROJECTILE_LAYER && other.gameObject.layer != PLAYER_BOMB_LAYER) return;

            ReceiveDamage(other.gameObject.layer == PLAYER_PROJECTILE_LAYER ? 1 : 30);

            if (other.gameObject.layer == PLAYER_PROJECTILE_LAYER)
                other.gameObject.SetActive(false);
        }

        private void ReceiveDamage(int damage)
        {
            if (!_canTakeDamage) return;

            _currentHealth = (_currentHealth - damage >= 0) ? _currentHealth - damage : 0;
            SoundEffectController.RequestSfx(SfxTypes.EnemyHit);
            GameEvents.OnPointsValueChange(10);

            if (_currentHealth >= 0)
                BossHealthUI.RequestHealthBarChange(_currentHealthInfo.barHealth, _currentHealth, 0.1f);

            if (_currentHealth <= 0)
            {
                _canTakeDamage = false;
                
                DropItems();
                SpecialEffectsManager.RequestBulletClearShockwave(transform.position, 5);
                SpecialEffectsManager.RequestExplosion(transform.position);
                SoundEffectController.RequestSfx(SfxTypes.BossExplosion);

                if (++_currentHealthBar >= _bossHealthBars)
                {
                    BossHealthUI.ToggleHealthBar(false);
                    StageController.CallNextStage();
                    StartCoroutine(Defeat());
                }
                else
                    StartCoroutine(InitiliazeNextPhase());
            }
        }

        private void ResetHealth()
        {
            _currentHealthInfo = _bossHealthInfo[_currentHealthBar];
            _currentHealth = _currentHealthInfo.barHealth;
            _hasDroppedItems = false;

            BossHealthUI.RequestHealthBarColorChange(_bossHealthBars - (_currentHealthBar + 1), true);
        }

        private void DropItems()
        {
            if (_hasDroppedItems)
                return;

            _hasDroppedItems = true;

            foreach (PossibleDrops possibleDrop in _currentHealthInfo.possibleDrops)
            {
                for (int i = 0; i < possibleDrop.timesToDrop; i++)
                {
                    if (!(Random.value <= possibleDrop.dropChance)) continue;

                    var originPosition = transform.position;
                    float xPosition = originPosition.x + Random.Range(-0.1f, 0.1f);
                    float yPosition = originPosition.y + Random.Range(-0.1f, 0.1f);
                    DropManager.RequestDrop(possibleDrop.dropType, new Vector3(xPosition, yPosition));
                }
            }
        }

        private IEnumerator InitiliazeNextPhase()
        {
            _canTakeDamage = false;

            ResetHealth();

            yield return StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));

            _canTakeDamage = true;
        }

        private IEnumerator Defeat()
        {
            GameEvents.OnPointsValueChange(50000);

            if(_destroyWhenDefeat)
                Destroy(gameObject);
            else
            {
                yield return StartCoroutine(_movementController.ReturnToInitialPosition());
                Destroy(gameObject);
            }
        }

        public void SetHealth(BossHealthInfo[] bossHealthInfo, bool destroyWhenDefeat)
        {
            _bossHealthInfo = bossHealthInfo;

            _bossHealthBars = _bossHealthInfo.Length;
            _currentHealthInfo = _bossHealthInfo[_currentHealthBar];
            _currentHealth = _currentHealthInfo.barHealth;

            _destroyWhenDefeat = destroyWhenDefeat;
        }

        public IEnumerator InitializeBoss()
        {
            BossHealthUI.RequestHealthBarColorChange(_bossHealthBars - (_currentHealthBar + 1), false);
            StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));

            yield return _canTakeDamageDelay;
            _canTakeDamage = true;
        }
    }
}