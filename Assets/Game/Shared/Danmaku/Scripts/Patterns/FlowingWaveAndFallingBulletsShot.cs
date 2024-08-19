using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class FlowingWaveAndFallingBulletsShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            Vector2 position = Vector2.zero;
            float xMaxPosition = 0.5f;
            float xPosition;
            float speed;
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                xPosition = Random.Range(-xMaxPosition, xMaxPosition);
                position = new Vector2(xPosition, transform.position.y);
                speed = shotSpeed;
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(position) : Random.Range(0,360);

                for (int k = 0; k < timesToShoot; k++)
                {
                    enemyProjectileManager.FireProjectile(projectileType, position, shotSpeed, angle);
                    speed += shotSpeedReduction;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                yield return delay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds innerDelay = new WaitForSeconds(0.075f);
            float yPosition = transform.position.y;
            float angle = 230;
            float angle2 = 220;
            float angleStep1 = 1.25f;
            float angleStep2 = 2.5f;
            float innerAngleStep = 60;
            Vector2 positionRight1 = new Vector2(0.4f, yPosition);
            Vector2 positionRight2 = new Vector2(0.2f, yPosition);
            Vector2 positionLeft1 = new Vector2(-0.2f, yPosition);
            Vector2 positionLeft2 = new Vector2(-0.4f, yPosition);

            for (int i = 0; i < timesToLoop; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    FireSecondaryShot(positionRight1, -angle2, innerAngleStep);
                    FireSecondaryShot(positionRight2, -angle, innerAngleStep);
                    FireSecondaryShot(positionLeft1, angle, -innerAngleStep);
                    FireSecondaryShot(positionLeft2, angle2, -innerAngleStep);

                    angle += angleStep1;
                    angle2 += angleStep2;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                    yield return innerDelay;
                }

                angleStep1 *= -1;
                angleStep2 *= -1;
            }
        }

        private void FireSecondaryShot(Vector2 position, float angle, float angleStep)
        {
            for (int k = 0; k < 3; k++)
            {
                enemyProjectileManager.GetFireProjectile(ProjectileType.EnemyBulletRed, position, 1.25f, angle);
                angle += angleStep;
            }
        }
    }
}