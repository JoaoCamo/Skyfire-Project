using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class RainShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();

            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.05f);
            WaitForSeconds shotFallDelay = new WaitForSeconds(1);
            
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        angle = Random.Range(0, 360);
                        speed = Random.Range(0.1f, shotSpeed);
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle));
                    }

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return shotFallDelay;

                StartShotFall(new List<ProjectileBase>(projectiles));
                projectiles.Clear();

                yield return delay;
            }
        }

        private void StartShotFall(List<ProjectileBase> projectiles)
        {
            foreach (ProjectileBase projectile in projectiles)
                projectile.transform.DORotate(new Vector3(0, 0, 180), 0.5f);
        }
    }
}