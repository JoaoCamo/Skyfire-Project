using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class FlowerShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float speed;
            float angleClockwise;
            float angleCounterClockwise;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angleClockwise = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
                angleCounterClockwise = angleClockwise + ( 360f / (timesToShoot * 2));
                speed = shotSpeed;

                for (int j = 0; j < 36; j++)
                {
                    innerAngle = angleClockwise;
                    speed += shotSpeedReduction;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }

                    innerAngle = angleCounterClockwise;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }

                    angleClockwise += 360f / 36f;
                    angleCounterClockwise -= 360f / 36f;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return delay;
                }
            }
        }
    }
}