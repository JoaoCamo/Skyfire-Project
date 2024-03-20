using System.Collections;
using UnityEngine;
using Game.Drop;
using Game.Stage;
using Game.Gameplay.UI;
using Game.Projectiles;

namespace Game.Enemy.Boss
{
    public class BossHealthController : MonoBehaviour
    {
        private BossHealthInfo[] _bossHealthInfo;

        private BossAttackController _attackController;

        private BossHealthInfo _currentHealthInfo;
        private int _bossHealthBars = 0;
        private int _currentHealthBar = 0;
        private int _currentHealth = 0;
        private bool _hasDroppedItems = false;
        private bool _canTakeDamage = false;

        private readonly WaitForSeconds _shockwaveFadeDelay = new WaitForSeconds(0.5f);
        private const int PLAYER_PROJECTILE_LAYER = 7;
        private const int PLAYER_BOMB_LAYER = 13;

        private void Awake()
        {
            _attackController = GetComponent<BossAttackController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PLAYER_PROJECTILE_LAYER && other.gameObject.layer != PLAYER_BOMB_LAYER) return;

            ReceiveDamage(other.gameObject.layer == PLAYER_PROJECTILE_LAYER ? 1 : 20);

            if (other.gameObject.layer == PLAYER_PROJECTILE_LAYER)
                other.gameObject.SetActive(false);
        }

        private void ReceiveDamage(int damage)
        {
            if (!_canTakeDamage) return;

            _currentHealth -= (_currentHealth - damage >= 0) ? damage : 0;

            if(_currentHealth >= 0)
                BossHealthUI.RequestHealthBarChange?.Invoke(_currentHealthInfo.barHealth, _currentHealth, 0.1f);

            if (_currentHealth <= 0)
            {
                _canTakeDamage = false;
                
                DropItems();

                if (++_currentHealthBar >= _bossHealthBars)
                {
                    EnemySpawner.RequestShockwave?.Invoke(transform.position ,2);
                    BossHealthUI.ToggleHealthBar?.Invoke(false);
                    EnemyProjectileManager.RequestFullClear?.Invoke();
                    
                    StageController.CallNextStage?.Invoke();
                    Destroy(gameObject);
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

            BossHealthUI.RequestHealthBarColorChange?.Invoke(_bossHealthBars - (_currentHealthBar + 1));
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
                    DropManager.RequestDrop?.Invoke(possibleDrop.dropType, new Vector3(xPosition, yPosition));
                }
            }
        }

        private IEnumerator InitiliazeNextPhase()
        {
            ResetHealth();
            EnemySpawner.RequestShockwave?.Invoke(transform.position, 0.75f);

            _canTakeDamage = false;

            yield return StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));

            _canTakeDamage = true;
        }

        public void SetHealth(BossHealthInfo[] bossHealthInfo)
        {
            _bossHealthInfo = bossHealthInfo;

            _bossHealthBars = _bossHealthInfo.Length;
            _currentHealthInfo = _bossHealthInfo[_currentHealthBar];
            _currentHealth = _currentHealthInfo.barHealth;
        }

        public void InitializeBoss()
        {
            _canTakeDamage = true;
            BossHealthUI.RequestHealthBarColorChange?.Invoke(_bossHealthBars - (_currentHealthBar+1));
            StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));
        }
    }
}