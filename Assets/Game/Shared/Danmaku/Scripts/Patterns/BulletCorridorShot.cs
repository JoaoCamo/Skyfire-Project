using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class BulletCorridorShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.1f);
            float speed;
            float angle;
            float innerAngle;
            bool invertAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 0;
                speed = shotSpeed;
                invertAngle = false;

                for (int j = 1; j <= 35; j++)
                {
                    innerAngle = angle;
                    speed += shotSpeedReduction;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }

                    angle += invertAngle ? 2f : -2f;

                    if (j % 18 == 0)
                        invertAngle = !invertAngle;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return delay;
            }
        }
    }
}