using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Danmaku.Patterns
{
    public class ExpandingHalfMoonShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds shotExpansionDelay = new WaitForSeconds(1);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 115) : 205;
                speed = shotSpeed;

                ProjectileBase[] mainProjectiles = new ProjectileBase[3];

                for (int j = 0; j < 3; j++)
                {
                    mainProjectiles[j] = enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle);
                    angle += 25;
                }

                yield return shotExpansionDelay;

                for (int j = 0; j < mainProjectiles.Length; j++)
                {
                    angle = mainProjectiles[j].transform.rotation.z + 90f;
                    speed = shotSpeed + shotSpeedReduction;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, mainProjectiles[j].transform.position, speed, angle);
                        angle += 180f / timesToShoot;
                    }
                }

                yield return delay;
            }
        }
    }
}