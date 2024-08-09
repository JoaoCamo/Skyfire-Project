using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class CircleSlowdownShot : DanmakuBase
    {
        private readonly WaitForSeconds _slowdownDelay = new WaitForSeconds(1);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();  
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float speed = shotSpeed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle));
                    angle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                StartCoroutine(SlowdownBullets(new List<ProjectileBase>(projectiles)));
                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator SlowdownBullets(List<ProjectileBase> projectiles)
        {
            yield return _slowdownDelay;

            foreach (ProjectileBase projectile in projectiles)
            {
                if(projectile.gameObject.activeSelf)
                    projectile.Speed = shotSpeedReduction;
            }
        }
    }
}