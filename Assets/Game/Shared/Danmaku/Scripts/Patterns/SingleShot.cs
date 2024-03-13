using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class SingleShot : EnemyAttackBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float speed;
            float angle;

            if (isInfiniteLoop)
            {
                while (true)
                {
                    for (int i = 0; i < timesToShoot; i++)
                    {
                        angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 180;
                        speed = shotSpeed;
                        for (int j = 0; j < timesToShoot; j++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }
                        yield return delay;
                    }
                }
            }
            else
            {
                for (int i = 0; i < timesToLoop; i++)
                {
                    for (int j = 0; j < timesToShoot; j++)
                    {
                        angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 180;
                        speed = shotSpeed;
                        for (int k = 0; k < timesToShoot; k++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }
                        yield return delay;
                    }
                }
            }
        }
    }
}