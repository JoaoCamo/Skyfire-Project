using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class FallingCircleShot : DanmakuBase
    {
        private readonly WaitForSeconds _bulletStopDelay = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _bulletFallDelay = new WaitForSeconds(0.5f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            int shotsToFire = (timesToShoot * 2);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < shotsToFire; j++)
                {
                    speed = shotSpeed;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle));
                        speed += shotSpeedReduction;
                    }
                    angle += 360f / shotsToFire;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                yield return shotDelay;
                StartCoroutine(DropBullets(new List<ProjectileBase>(projectiles)));

                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator DropBullets(List<ProjectileBase> projectiles)
        {
            yield return _bulletStopDelay;

            foreach (ProjectileBase projectile in projectiles)
            {
                DOTween.To(() => projectile.Speed, x => projectile.Speed = x, 0, 0.5f).SetEase(Ease.Linear);
            }

            yield return _bulletFallDelay;

            float speedMultiplier = Random.value > 0.2f ? 1 : 0.5f;

            foreach (ProjectileBase projectile in projectiles)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, 180);
                projectile.Speed = shotSpeed * speedMultiplier;
            }
        }
    }
}