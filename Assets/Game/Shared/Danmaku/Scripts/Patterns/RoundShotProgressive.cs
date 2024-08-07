using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class RoundShotProgressive : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float speed;
            float angle;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 0;
                speed = shotSpeed;

                for (int j = 0; j < 36; j++)
                {
                    innerAngle = angle;
                    speed += shotSpeedReduction;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }
                    angle += 360f / 36f;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return delay;
                }
            }
        }
    }
}