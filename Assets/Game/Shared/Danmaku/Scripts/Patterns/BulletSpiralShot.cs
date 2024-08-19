using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class BulletSpiralShot : DanmakuBase
    {
        public override IEnumerator Shoot()
        {
            List<Vector2> shotOriginPosition = new List<Vector2>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.05f);
            float radius = 0.75f;
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                Vector3 targetPosition = PlayerMovement.PlayerTransform.position;
                shotOriginPosition = GetPositions(targetPosition, radius, timesToShoot);

                for (int j = 0; j < shotOriginPosition.Count; j++)
                {
                    angle = AimAtTarget(shotOriginPosition[j], targetPosition);

                    for (int k = 0; k < 18; k++)
                    {
                        enemyProjectileManager.GetFireProjectile(projectileType, shotOriginPosition[j], shotSpeed, angle);
                        angle += 360 / 18;
                    }

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                    yield return innerDelay;
                }

                yield return delay;
            }
        }

        private float AimAtTarget(Vector3 originPosition, Vector3 targetPosition, float angleCorrection = 90)
        {
            Vector2 vectorToTarget = targetPosition - originPosition;
            return Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - angleCorrection;
        }

        private List<Vector2> GetPositions(Vector2 center, float radius, int numberOfPoints)
        {
            List<Vector2> points = new List<Vector2>();

            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = i * Mathf.PI * 2 / numberOfPoints;

                float x = center.x + Mathf.Cos(angle) * radius;
                float y = center.y + Mathf.Sin(angle) * radius;

                points.Add(new Vector2(x, y));
            }
            for (int i = 0; i < numberOfPoints; i++)
            {
                points.Add(points[i]);
            }

            return points;
        }
    }
}