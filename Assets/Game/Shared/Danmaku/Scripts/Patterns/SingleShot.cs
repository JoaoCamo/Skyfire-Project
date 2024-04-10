using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class SingleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float speed;
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

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