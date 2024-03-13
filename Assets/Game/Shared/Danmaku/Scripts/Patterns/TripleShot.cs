using System.Collections;
using Game.Projectiles;
using UnityEngine;

namespace Game.Danmaku.Patterns
{
    public class TripleShot : EnemyAttackBase
    {
        public override IEnumerator Shoot()
        {
            float speed;
            float angle;
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
        
            if(isInfiniteLoop)
            {
                while(true)
                {
                    angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 115) : 205;

                    for (int i = 0; i < 3; i++)
                    {
                        speed = shotSpeed;

                        for (int j = 0; j < timesToShoot; j++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }

                        angle += 25f;
                    }

                    yield return delay;
                }
            }
            else
            {
                for (int i = 0; i < timesToLoop; i++)
                {
                    angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 115) : 205;

                    for (int j = 0; j < 3; j++)
                    {
                        speed = shotSpeed;

                        for (int k = 0; k < timesToShoot; k++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }

                        angle += 25f;
                    }

                    yield return delay;
                }
            }
        }
    }
}