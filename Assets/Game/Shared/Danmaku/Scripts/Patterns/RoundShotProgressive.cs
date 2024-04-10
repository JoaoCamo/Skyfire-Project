using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class RoundShotProgressive : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 9999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 180;

                for (int j = 0; j < 64; j++)
                {
                    innerAngle = angle;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }
                    angle += 360f / 64f;

                    yield return delay;
                }
            }
        }
    }
}