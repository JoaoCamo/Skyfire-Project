using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class BulletScatterAndCircleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    angle = Random.Range(0, 360);
                    speed = Random.Range(0.1f, shotSpeed);
                    enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds delay = new WaitForSeconds(3);
            float angle;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    enemyProjectileManager.FireProjectile(ProjectileType.EnemyOrbRed, transform.position, 0.3f, angle);
                    angle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }
    }
}