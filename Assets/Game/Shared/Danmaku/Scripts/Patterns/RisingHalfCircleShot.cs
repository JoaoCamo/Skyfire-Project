using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class RisingHalfCircleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.025f);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : GetStartAngle(i);
                speed = shotSpeed;

                for (int j = 0; j < timesToShoot; j++)
                {
                    enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                    angle += i % 2 == 0 ? 10 : -10;
                    speed += shotSpeedReduction;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return delay;
            }
        }

        private float GetStartAngle(int i)
        {
            return i % 2 == 0 ? 90 : 270;
        }
    }
}