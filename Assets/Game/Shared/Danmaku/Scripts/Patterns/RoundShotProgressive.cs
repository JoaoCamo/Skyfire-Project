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

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < 32; j++)
                {
                    innerAngle = angle;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }
                    angle += 360f / 32f;

                    yield return delay;
                }
            }
        }
    }
}