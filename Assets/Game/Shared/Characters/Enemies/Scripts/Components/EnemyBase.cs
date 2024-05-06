using UnityEngine;
using Game.Danmaku;
using Game.Projectiles;
using Game.Gameplay.Effects;
using Game.Audio;

namespace Game.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        private EnemyHealth health;
        private EnemyMovement movement;
        private EnemyAttackController attackController;

        public EnemyType EnemyType => enemyType;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
            movement = GetComponent<EnemyMovement>();
            attackController = GetComponent<EnemyAttackController>();
        }

        public void SetEnemy(EnemyAttackInfo enemyAttackInfo, EnemyMovementInfo enemyMovementInfo, EnemyInitialPosition enemyInitialPosition, PossibleDrops[] possibleDrops, int enemyHealth, EnemyProjectileManager enemyProjectileManager)
        {
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);

            health.SetHealth(enemyHealth, possibleDrops);
            movement.SetPosition(enemyInitialPosition);
            movement.SetMovement(enemyMovementInfo);
            attackController.SetAttack(enemyAttackInfo, enemyProjectileManager);

        }

        public void UpdatePosition()
        {
            movement.UpdatePosition();
        }

        public void ForceClear()
        {
            if (!gameObject.activeSelf) return;

            SoundEffectController.RequestSfx(SfxTypes.EnemyExplosion);
            SpecialEffectsManager.RequestShockwave(transform.position);
            gameObject.SetActive(false);
        }
    }
}