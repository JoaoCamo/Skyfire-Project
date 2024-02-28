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
                        speed = 1;

                        for (int j = 0; j < timesToShoot; j++)
                        {
                            EnemyProjectileManager.RequestBullet?.Invoke(projectileType, transform.position, speed, 1, angle);
                            speed -= 0.1f;
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
                        speed = 1;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            EnemyProjectileManager.RequestBullet?.Invoke(projectileType, transform.position, speed, 1, angle);
                            speed -= 0.1f;
                        }

                        angle += 360f / 16f;
                    }

                    yield return delay;
                }
            }
        }
    }
}