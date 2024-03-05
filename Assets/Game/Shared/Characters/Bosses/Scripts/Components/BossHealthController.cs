using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Drop;
using Game.Stage;
using Game.Gameplay.UI;

namespace Game.Enemy.Boss
{
    public class BossHealthController : MonoBehaviour
    {
        [SerializeField] private GameObject shockwavePrefab;

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
            BossHealthUI.RequestHealthBarChange?.Invoke(_currentHealthInfo.barHealth, _currentHealth, 0.1f);

            if (_currentHealth <= 0)
            {
                _canTakeDamage = false;
                
                DropItems();

                if (++_currentHealthBar >= _bossHealthBars)
                {
                    StageController.CallNextStage?.Invoke();
                    BossHealthUI.ToggleHealthBar?.Invoke(false);
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

            BossHealthUI.RequestHealthBarChange?.Invoke(_currentHealthInfo.barHealth, _currentHealth, 1);
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

        private IEnumerator StartShockwave()
        {
            SpriteRenderer shockwaveSpriteRenderer = Instantiate(shockwavePrefab, transform).GetComponent<SpriteRenderer>();
            shockwaveSpriteRenderer.transform.DOScale(1, 1);

            yield return _shockwaveFadeDelay;

            Color originalColor = shockwaveSpriteRenderer.color;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

            shockwaveSpriteRenderer.DOColor(newColor, 0.25f);

            yield return _shockwaveFadeDelay;

            Destroy(shockwaveSpriteRenderer.gameObject);
        }

        private IEnumerator InitiliazeNextPhase()
        {
            ResetHealth();
            StartCoroutine(StartShockwave());

            _canTakeDamage = false;

            yield return StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));

            _canTakeDamage = true;
        }

        public void SetHealth(BossHealthInfo[] bossHealthInfo)
        {
            _bossHealthInfo = bossHealthInfo;

            _bossHealthBars = (_bossHealthInfo.Length);
            _currentHealthInfo = _bossHealthInfo[_currentHealthBar];
            _currentHealth = _currentHealthInfo.barHealth;
        }

        public void InitializeBoss()
        {
            _canTakeDamage = true;
            StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));
        }
    }
}