using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class ProgressiveRoundReturnShot : DanmakuBase
    {
        private readonly WaitForSeconds _returnDelay = new WaitForSeconds(1.75f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.1f);
            float speed;
            float angle;
            float innerAngle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : 0;
                speed = shotSpeed;

                for (int j = 0; j < 36; j++)
                {
                    innerAngle = angle;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, innerAngle));
                        innerAngle += 360f / timesToShoot;
                    }

                    StartCoroutine(ReturnBullets(new List<ProjectileBase>(projectiles)));
                    projectiles.Clear();

                    angle += 10;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return delay;
                }
            }
        }

        private IEnumerator ReturnBullets(List<ProjectileBase> projectiles)
        {
            yield return _returnDelay;

            foreach (ProjectileBase projectile in projectiles)
            {
                if(projectile.gameObject.activeSelf)
                {
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    projectile.Speed = shotSpeedReduction;
                }
            }
        }
    }
}