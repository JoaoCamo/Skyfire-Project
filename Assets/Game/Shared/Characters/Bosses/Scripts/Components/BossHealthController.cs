using UnityEngine;
using Game.Drop;

namespace Game.Enemy.Boss
{
    public class BossHealthController : MonoBehaviour
    {
        [SerializeField] private BossHealthInfo[] bossHealthInfo;

        private BossAttackController _attackController;

        private BossHealthInfo _currentHealthInfo;
        private int _bossHealthBars = 0;
        private int _currentHealthBar = 0;
        private int _currentHealth = 0;
        private bool _hasDroppedItems = false;
        private bool _canChangeHealthBar = true;

        private const int PLAYER_PROJECTILE_LAYER = 7;
        private const int PLAYER_BOMB_LAYER = 13;

        private void Awake()
        {
            _attackController = GetComponent<BossAttackController>();

            _bossHealthBars = (bossHealthInfo.Length);
            _currentHealthInfo = bossHealthInfo[_currentHealthBar];
            _currentHealth = _currentHealthInfo.barHealth;
        }

        private void Start()
        {
            StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));
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
            _currentHealth -= (_currentHealth - damage >= 0) ? damage : 0;

            if (_currentHealth <= 0 && _canChangeHealthBar)
            {
                _canChangeHealthBar = false;
                
                DropItems();
                
                if(++_currentHealthBar >= _bossHealthBars)
                    Destroy(gameObject);
                else
                {
                    ResetHealth();
                    StartCoroutine(_attackController.InitializeNextAttack(_currentHealthBar));
                }
            }
        }

        private void ResetHealth()
        {
            _currentHealthInfo = bossHealthInfo[_currentHealthBar];
            _currentHealth = _currentHealthInfo.barHealth;
            _hasDroppedItems = false;
            _canChangeHealthBar = true;
        }

        private void DropItems()
        {
            if (_hasDroppedItems || !_currentHealthInfo.willDropItems)
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
    }
}