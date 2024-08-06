using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class DoubleCircleHomingShot : DanmakuBase
    {
        private readonly WaitForSeconds _aimBulletDelay = new WaitForSeconds(1.5f);
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(2.5f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot(0.5f, delay));

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0,360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    projectiles.Add(enemyProjectileManager.GetFireProjectile(ProjectileType.EnemyOrbRed, transform.position, (shotSpeed / 2), angle));
                    angle += 360f / timesToShoot;
                }

                StartCoroutine(AimBullets(new List<ProjectileBase>(projectiles)));
                projectiles.Clear();

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return _primaryShotDelay;
            }
        }

        private IEnumerator SecondaryShot(float startDelay, WaitForSeconds delay)
        {
            yield return new WaitForSeconds(startDelay);
            float angle;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, angle);
                    angle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }

        private IEnumerator AimBullets(List<ProjectileBase> projectiles)
        {
            yield return _aimBulletDelay;
            
            foreach (ProjectileBase projectile in projectiles)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, EnemyProjectileManager.AimAtPlayer(projectile.transform.position));
                projectile.Speed = shotSpeed;
            }
        }
    }
}