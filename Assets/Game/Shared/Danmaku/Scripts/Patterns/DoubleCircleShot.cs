using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class DoubleCircleShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            Vector2 originPositionLeft = new Vector2(-0.3f, transform.position.y);
            Vector2 originPositionRight = new Vector2(0.3f, transform.position.y);
            Vector2 originPosition;
            float angle;
            float speed = shotSpeed;
            bool invert = false;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                originPosition = invert ? originPositionLeft : originPositionRight;
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(originPosition) : Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    enemyProjectileManager.FireProjectile(projectileType, originPosition, speed, angle);
                    angle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                speed += shotSpeedReduction;

                invert = !invert;

                yield return delay;
            }
        }
    }
}