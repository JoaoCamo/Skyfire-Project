using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class FallingPetalsShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.035f);
            float speed;
            float angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 0;
            float innerAngle;
            bool invertAngle = false;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                speed = shotSpeed;
                invertAngle = Random.value > 0.25f;

                for (int j = 0; j < 9; j++)
                {
                    
                    speed += shotSpeedReduction;

                    for (int k = 0; k < 6; k++)
                    {
                        innerAngle = angle;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, innerAngle);
                            innerAngle += 360f / timesToShoot;
                        }

                        SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                        angle += invertAngle ? -2 : 2;
                        yield return innerDelay;
                    }

                    angle += invertAngle ? -15 : 15;
                }

                yield return delay;
            }
        }
    }
}