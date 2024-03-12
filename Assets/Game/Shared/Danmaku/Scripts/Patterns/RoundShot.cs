using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku
{
    public class RoundShot : EnemyAttackBase
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
                    angle = EnemyProjectileManager.AimAtPlayer(transform.position);

                    for (int i = 0; i < 16; i++)
                    {
                        speed = shotSpeed;

                        for (int j = 0; j < timesToShoot; j++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }
                        angle += 360f / 16f;
                    }

                    yield return delay;
                }
            }
            else
            {
                for (int i = 0; i < timesToLoop; i++)
                {
                    angle = EnemyProjectileManager.AimAtPlayer(transform.position);

                    for (int j = 0; j < 16; j++)
                    {
                        speed = shotSpeed;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }

                        angle += 360f / 16f;
                    }

                    yield return delay;
                }
            }
        }
    }
}