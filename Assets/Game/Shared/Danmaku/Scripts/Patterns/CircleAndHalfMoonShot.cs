using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
   public class CircleAndHalfMoonShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.5f);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
                speed = shotSpeed;

                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 36; k++)
                    {
                        speed = shotSpeed;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }
                        angle += 360f / 36f;
                    }

                    speed = shotSpeed + 0.25f;
                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return delay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds delay = new WaitForSeconds(1);
            float angle;
            float speed;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 180) : Random.Range(0, 360);

                for (int j = 0; j < 9; j++)
                {
                    speed = shotSpeed;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(ProjectileType.EnemyDonetGreen, transform.position, shotSpeed, angle);
                        speed += shotSpeedReduction;
                    }
                    angle += 180f / 9f;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }
    }
}