using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class DoubleCircleSlowdownShot : DanmakuBase
    {
        private readonly WaitForSeconds _slowdownDelay = new WaitForSeconds(0.9f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            Vector2 originPositionLeft = new Vector2(-0.3f, transform.position.y);
            Vector2 originPositionRight = new Vector2(0.3f, transform.position.y);
            float angleLeft;
            float angleRight;
            float speed = shotSpeed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angleLeft = isAimed ? EnemyProjectileManager.AimAtPlayer(originPositionLeft) : Random.Range(0, 360);
                angleRight = isAimed ? EnemyProjectileManager.AimAtPlayer(originPositionRight) : Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, originPositionLeft, speed, angleLeft));
                    projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, originPositionRight, speed, angleRight));
                    angleLeft += 360f / timesToShoot;
                    angleRight += 360f / timesToShoot;
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
                if (projectile.gameObject.activeSelf)
                    projectile.Speed = shotSpeedReduction;
            }
        }
    }
}