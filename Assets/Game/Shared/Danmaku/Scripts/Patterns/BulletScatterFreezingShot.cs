using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class BulletScatterFreezingShot : DanmakuBase
    {
        private readonly WaitForSeconds _freezeBulletDelay = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _unfreezeBulletDelay = new WaitForSeconds(1.5f);
        private readonly WaitForSeconds _bulletFireDelay = new WaitForSeconds(0.1f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                for (int j = 0; j < timesToShoot; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        angle = Random.Range(0, 360);
                        speed = Random.Range(0.1f, shotSpeed);
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle));
                    }

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return _bulletFireDelay;
                }

                StartCoroutine(FreezeBullets(new List<ProjectileBase>(projectiles)));
                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator FreezeBullets(List<ProjectileBase> projectiles)
        {
            yield return _freezeBulletDelay;

            foreach (ProjectileBase projectile in projectiles)
                projectile.Speed = 0;

            yield return _unfreezeBulletDelay;

            foreach (ProjectileBase projectile in projectiles)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0,360));
                projectile.Speed = (shotSpeed / 4);
            }
        }
    }
}