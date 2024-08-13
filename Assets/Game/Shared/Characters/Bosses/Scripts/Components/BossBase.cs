using UnityEngine;
using Game.Gameplay.UI;
using Game.Projectiles;

namespace Game.Enemy.Boss
{
    public class BossBase : MonoBehaviour
    {
        private BossAttackController _attackController;
        private BossHealthController _healthController;

        private void Awake()
        {
            _attackController = GetComponent<BossAttackController>();
            _healthController = GetComponent<BossHealthController>();
        }

        public void SetProjectileManager(EnemyProjectileManager enemyProjectileManager)
        {
            _attackController.EnemyProjectileManager = enemyProjectileManager;
        }

        public void StartBossBattle(BossInfo bossInfo)
        {
            BossHealthUI.ToggleHealthBar(true);
            _attackController.BossAttackInfo = bossInfo.bossAttackInfo;
            _healthController.SetHealth(bossInfo.bossHealthInfo, bossInfo.destroyWhenDefeat);
            StartCoroutine(_healthController.InitializeBoss());
        }
    }
}