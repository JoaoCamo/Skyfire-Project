using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class FlowerAndCircleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float speed;
            float angleClockwise;
            float angleCounterClockwise;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                angleClockwise = 45;
                angleCounterClockwise = angleClockwise;
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

                    angleClockwise += 2f;
                    angleCounterClockwise -= 2f;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return delay;
                }
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds delay = new WaitForSeconds(2);
            int timesToFire = timesToShoot * 3;
            float angle;
            float speed = shotSpeed / 2;

            for (int i = 0; i < timesToLoop; i++)
            {
                yield return delay;

                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < timesToFire; j++)
                {
                    enemyProjectileManager.FireProjectile(ProjectileType.EnemyBulletTwoRed, transform.position, speed, angle);
                    angle += 360f / timesToFire;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
            }
        }
    }
}