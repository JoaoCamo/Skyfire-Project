using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class LWShot : DanmakuBase
    {
        private readonly WaitForSeconds _secondaryShotStartDelay = new WaitForSeconds(1.5f);
        private readonly WaitForSeconds _bulletAimDelay = new WaitForSeconds(1.5f);

        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angleClockwise;
            float angleCounterClockwise;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                angleClockwise = 45;
                angleCounterClockwise = angleClockwise;

                for (int j = 0; j < 36; j++)
                {
                    innerAngle = angleClockwise;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }

                    innerAngle = angleCounterClockwise;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, innerAngle);
                        innerAngle += 360f / timesToShoot;
                    }

                    angleClockwise += 5f;
                    angleCounterClockwise -= 5f;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return delay;
                }
            }
        }

        private IEnumerator SecondaryShot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(1.25f);
            WaitForSeconds innerDelay = new WaitForSeconds(0.075f);
            float originAngle = 100;
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            yield return _secondaryShotStartDelay;

            for (int i = 0; i < timesToLoop; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    angle = originAngle;

                    for (int k = 0; k < 4; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(ProjectileType.EnemyBulletTwoBlue, transform.position, shotSpeedReduction, -angle));
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(ProjectileType.EnemyBulletTwoBlue, transform.position, shotSpeedReduction, angle));
                        angle += 2.5f;
                    }

                    StartCoroutine(AimBullets(new List<ProjectileBase>(projectiles)));
                    projectiles.Clear();

                    yield return innerDelay;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                yield return delay;
            }
        }

        private IEnumerator AimBullets(List<ProjectileBase> projectiles)
        {
            yield return _bulletAimDelay;

            foreach (ProjectileBase projectile in projectiles)
                projectile.transform.rotation = Quaternion.Euler(0, 0, EnemyProjectileManager.AimAtPlayer(projectile.transform.position));
        }
    }
}