using System.Collections;
using Game.Projectiles;
using UnityEngine;

namespace Game.Danmaku
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
                    angle = EnemyProjectileManager.AimAtPlayer(transform.position, 125);
                    
                    for(int i = 0; i < 3; i++)
                    {
                        speed = 1;

                        for (int j = 0; j < timesToShoot; j++)
                        {
                            EnemyProjectileManager.RequestBullet?.Invoke(projectileType, transform.position, speed, 1, angle);
                            speed -= 0.1f;
                        }

                        angle += 35f;
                    }

                    yield return delay;
                }
            }
            else
            {
                for (int i = 0; i < timesToLoop; i++)
                {
                    angle = EnemyProjectileManager.AimAtPlayer(transform.position, 125);

                    for (int j = 0; j < 3; j++)
                    {
                        speed = 1;

                        for (int k = 0; k < timesToShoot; k++)
                        {
                            EnemyProjectileManager.RequestBullet?.Invoke(projectileType, transform.position, speed, 1, angle);
                            speed -= 0.1f;
                        }

                        angle += 35f;
                    }

                    yield return delay;
                }
            }
        }
    }
}