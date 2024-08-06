using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class ProgressiveRoundStopShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds fireShotDelay = new WaitForSeconds(0.1f);
            float speed;
            float angle;
            float innerAngle;

            List<ProjectileBase> projectiles = new List<ProjectileBase>();

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
                speed = shotSpeed;

                for (int j = 0; j < timesToShoot; j++)
                {
                    innerAngle = angle;

                    for (int k = 0; k < 4; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, innerAngle));
                        innerAngle = angle + (360f / 4f * (k + 1));
                    }

                    angle += 10;
                    speed += shotSpeedReduction;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return fireShotDelay;
                }

                StartCoroutine(BulletStopCoroutine(new List<ProjectileBase>(projectiles)));

                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator BulletStopCoroutine(List<ProjectileBase> projectilesToStop)
        {
            WaitForSeconds shotStopDelay = new WaitForSeconds(0.15f);
            WaitForSeconds bulletStopDelay = new WaitForSeconds(0.5f);

            yield return bulletStopDelay;

            for (int i = 4; i < projectilesToStop.Count; i += 4)
            {
                for (int j = 0; j < 4; j++)
                {
                    projectilesToStop[i].Speed *= 0.25f;
                    i++;
                }

                yield return shotStopDelay;
            }

            for (int i = 4; i < projectilesToStop.Count; i += 4)
            {
                for (int j = 0; j < 4; j++)
                {
                    projectilesToStop[i].transform.rotation = Quaternion.Euler(0, 0, 180);
                    projectilesToStop[i].Speed = shotSpeed;
                    i++;
                }

                yield return shotStopDelay;
            }
        }
    }
}