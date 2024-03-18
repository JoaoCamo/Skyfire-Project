using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku
{
    public class DanmakuBase : MonoBehaviour
    {
        protected EnemyProjectileManager enemyProjectileManager;

        protected ProjectileType projectileType;
        protected bool isAimed;
        protected bool isInfiniteLoop;
        protected int timesToLoop;
        protected int timesToShoot;
        protected float shotDelay;
        protected float shotSpeed;
        protected float shotSpeedReduction;

        public void SetShot(EnemyAttackInfo enemyAttack, EnemyProjectileManager projectileManager)
        {
            this.enemyProjectileManager = projectileManager;
            this.projectileType = enemyAttack.projectileType;
            this.isAimed = enemyAttack.isAimed;
            this.isInfiniteLoop = enemyAttack.isInfiniteLoop;
            this.timesToLoop = enemyAttack.timesToLoop;
            this.timesToShoot = enemyAttack.timesToShoot;
            this.shotDelay = enemyAttack.shotDelay;
            this.shotSpeed = enemyAttack.shotSpeed;
            this.shotSpeedReduction = enemyAttack.shotSpeedReduction;
        }

        public virtual IEnumerator Shoot()
        {
            yield return null;
        }
    }
}