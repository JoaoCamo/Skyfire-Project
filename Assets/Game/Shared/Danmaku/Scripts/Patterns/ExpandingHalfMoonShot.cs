using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class ExpandingHalfMoonShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds shotActionDelay = new WaitForSeconds(0.5f);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 140) : 230;
                speed = shotSpeed;

                ProjectileBase[] mainProjectiles = new ProjectileBase[3];

                for (int j = 0; j < 3; j++)
                {
                    mainProjectiles[j] = enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle);
                    angle += 50;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return shotActionDelay;

                foreach (ProjectileBase projectile in mainProjectiles)
                    projectile.Speed *= 0.5f;

                yield return shotActionDelay;

                for (int j = 0; j < mainProjectiles.Length; j++)
                {
                    angle = mainProjectiles[j].transform.rotation.z + 90f;
                    mainProjectiles[j].Speed = shotSpeed;

                    for (int k = 0; k < 6; k++)
                    {
                        speed = shotSpeed + shotSpeedReduction;

                        for (int z = 0; z < timesToShoot; z++)
                        {
                            enemyProjectileManager.FireProjectile(projectileType, mainProjectiles[j].transform.position, speed, angle);
                            speed += shotSpeedReduction;
                        }

                        angle += 180f / 6f;
                    }
                }

                yield return delay;
            }
        }
    }
}