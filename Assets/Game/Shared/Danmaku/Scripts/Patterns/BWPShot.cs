using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class BWPShot : DanmakuBase
    {
        private bool _canShoot = false;

        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
            float innerAngle;
            float angleStep = -5;
            _canShoot = true;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            while(_canShoot)
            {
                angle += angleStep;
                innerAngle = angle;

                for (int j = 0; j < timesToShoot; j++)
                {
                    enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, innerAngle);
                    innerAngle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                angleStep += 0.2f;

                yield return delay;
            }
        }
    }
}