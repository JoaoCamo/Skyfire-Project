using System.Collections;
using UnityEngine;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class BulletScatterShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            float speed;
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                for (int k = 0; k < timesToShoot; k++)
                {
                    angle = Random.Range(0,360);
                    speed = Random.Range(0.1f, shotSpeed);
                    enemyProjectileManager.FireProjectile(projectileType, transform.position, speed, angle);
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }
    }
}