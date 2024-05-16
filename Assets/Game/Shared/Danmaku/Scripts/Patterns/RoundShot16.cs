using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class RoundShot16 : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0,360);

                for (int j = 0; j < 16; j++)
                {
                    speed = shotSpeed;

                    for (int k = 0; k < timesToShoot; k++)
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