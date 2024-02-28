using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku
{
    public class EnemyAttackBase : MonoBehaviour
    {
        protected ProjectileType projectileType;
        protected bool isInfiniteLoop;
        protected int timesToLoop;
        protected int timesToShoot;
        protected float shotDelay;

        public void SetShot(EnemyAttackInfo enemyAttack)
        {
            this.projectileType = enemyAttack.projectileType;
            this.isInfiniteLoop = enemyAttack.isInfiniteLoop;
            this.timesToLoop = enemyAttack.timesToLoop;
            this.timesToShoot = enemyAttack.timesToShoot;
            this.shotDelay = enemyAttack.shotDelay;
        }

        public virtual IEnumerator Shoot()
        {
            yield return null;
        }
    }
}