using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class TripleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            float speed;
            float angle;
            WaitForSeconds delay = new WaitForSeconds(shotDelay);

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 115) : Random.Range(90,270);

                for (int j = 0; j < 3; j++)
                {
                    speed = shotSpeed;

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                        speed += shotSpeedReduction;
                    }

                    angle += 25f;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }
    }
}