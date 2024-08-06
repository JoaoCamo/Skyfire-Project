using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class CircleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
                speed = shotSpeed;

                for (int j = 0; j < timesToShoot; j++)
                {
                    enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                    speed += shotSpeedReduction;
                    angle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }
    }
}