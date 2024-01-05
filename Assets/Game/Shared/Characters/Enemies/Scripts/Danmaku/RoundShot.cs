using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Enemy.Attack
{
    public class RoundShot : EnemyAttackBase
    {
        private WaitForSeconds _delay = new WaitForSeconds(1.25f);

        protected override IEnumerator Shoot()
        {
            float angle;
            float speed;

            if (isInfiniteLoop)
            {
                while (true)
                {
                    angle = 0;

                    for (int i = 0; i < 16; i++)
                    {
                        speed = 1;

                        for (int j = 0; j < timesToShoot; j++)
                        {
                            EnemyProjectileManager.RequestBullet?.Invoke(projectileType, transform.position, speed, 1, Quaternion.Euler(0, 0, angle));
                            speed -= 0.1f;
                        }
                        angle += 360f / 16f;
                    }

                    yield return _delay;
                }
            }
            else
            {
                for (int i = 0; i < timesToLoop; i++)
                {
                    angle = 0;

                    for (int j = 0; j < 16; j++)
                    {
                        speed = 1;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            EnemyProjectileManager.RequestBullet?.Invoke(projectileType, transform.position, speed, 1, Quaternion.Euler(0, 0, angle));
                            speed -= 0.1f;
                        }

                        angle += 360f / 16f;
                    }

                    yield return _delay;
                }
            }
        }
    }
}