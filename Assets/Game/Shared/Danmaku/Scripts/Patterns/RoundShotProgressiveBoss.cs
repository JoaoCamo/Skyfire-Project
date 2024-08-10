using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class RoundShotProgressiveBoss : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.05f);
            float speed;
            float angle;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 0;

                for (int j = 0; j < 27; j++)
                {
                    innerAngle = angle;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        speed = shotSpeed;

                        for (int l = 0; l < 2; l++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, innerAngle);
                            speed += shotSpeedReduction;
                        }
                        
                        innerAngle += 360f / timesToShoot;
                    }
                    angle += 5;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return delay;
            }
        }
    }
}