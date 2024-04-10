using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class DoubleKite : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angleFront;
            float angleBack;
            float angleStep = 90/timesToShoot;
            float angleMultiplier;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angleFront = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
                angleBack = angleFront + 180;
                angleMultiplier = 0;
                speed = shotSpeed;

                for (int j = 0; j < timesToShoot; j++)
                {
                    if (j == 0)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angleFront);
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angleBack);
                    }
                    else
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angleFront + (angleStep * angleMultiplier));
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angleFront - (angleStep * angleMultiplier));

                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angleBack + (angleStep * angleMultiplier));
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angleBack - (angleStep * angleMultiplier));
                    }

                    speed += shotSpeedReduction;
                    angleMultiplier++;
                }

                yield return delay;
            }
        }
    }
}