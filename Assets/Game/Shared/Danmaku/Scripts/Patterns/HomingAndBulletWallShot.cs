using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Projectiles;
using Game.Audio;
using Game.Player;

namespace Game.Danmaku.Patterns
{
    public class HomingAndBulletWallShot : DanmakuBase
    {
        private readonly WaitForSeconds _bulletAimDelay = new WaitForSeconds(0.75f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.1f);
            Vector3 targetPosition;
            float originAngle1 = 50;
            float originAngle2 = 70;
            float angle;
            bool invert = false;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                targetPosition = PlayerMovement.PlayerTransform.position;

                for (int j = 0; j < 8; j++)
                {
                    angle = invert ? originAngle2 : originAngle1;

                    for (int k = 0; k < 4; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, shotSpeed, -angle));
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, shotSpeed, angle));
                        angle += 10;
                    }

                    StartCoroutine(AimBullets(new List<ProjectileBase>(projectiles), targetPosition));
                    projectiles.Clear();

                    yield return innerDelay;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                invert = !invert;

                yield return delay;
            }
        }

        private IEnumerator AimBullets(List<ProjectileBase> projectiles, Vector3 targetPosition)
        {
            foreach (ProjectileBase projectile in projectiles)
                DOTween.To(() => projectile.Speed, x => projectile.Speed = x, 0, 0.5f).SetEase(Ease.Linear);

            yield return _bulletAimDelay;

            foreach (ProjectileBase projectile in projectiles)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, AimAtTarget(projectile.transform.position, targetPosition));
                projectile.Speed = shotSpeedReduction;
            }
        }

        private float AimAtTarget(Vector3 originPosition, Vector3 targetPosition, float angleCorrection = 90)
        {
            Vector2 vectorToTarget = targetPosition - originPosition;
            return Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - angleCorrection;
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds delay = new WaitForSeconds(1.5f);
            WaitForSeconds innerDelay = new WaitForSeconds(0.075f);
            float angleStep = 360f / timesToShoot;
            float angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, (90 - (angleStep / 2))) : Random.Range(0, 360);
            float innerAngle;

            for (int i = 0; i < timesToLoop; i++)
            {
                innerAngle = angle;

                for (int k = 0; k < timesToShoot; k++)
                {
                    enemyProjectileManager.GetFireProjectile(ProjectileType.EnemyBulletTwoBlue, transform.position, 1.25f, innerAngle);
                    innerAngle += angleStep;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                yield return innerDelay;
            }
        }
    }
}