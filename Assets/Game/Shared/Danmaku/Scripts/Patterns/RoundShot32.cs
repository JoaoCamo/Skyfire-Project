using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class RoundShot32 : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float speed;

            if (isInfiniteLoop)
            {
                while (true)
                {
                    angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 180;

                    for (int i = 0; i < 32; i++)
                    {
                        speed = shotSpeed;

                        for (int j = 0; j < timesToShoot; j++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }
                        angle += 360f / 32f;
                    }

                    yield return delay;
                }
            }
            else
            {
                for (int i = 0; i < timesToLoop; i++)
                {
                    angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 180;

                    for (int j = 0; j < 32; j++)
                    {
                        speed = shotSpeed;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }

                        angle += 360f / 32f;
                    }

                    yield return delay;
                }
            }
        }
    }
}