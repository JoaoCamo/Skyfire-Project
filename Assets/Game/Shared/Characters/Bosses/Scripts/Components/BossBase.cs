using UnityEngine;
using Game.Gameplay.UI;

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

        public void StartBossBattle(BossInfo bossInfo)
        {
            BossHealthUI.ToggleHealthBar?.Invoke(true);
            _attackController.BossAttackInfo = bossInfo.bossAttackInfo;
            _healthController.SetHealth(bossInfo.bossHealthInfo);
            _healthController.InitializeBoss();
        }
    }
}