using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class ClosingCircle : DanmakuBase
    {
        private readonly WaitForSeconds _closingDelay = new WaitForSeconds(2.5f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            ProjectileBase currentProjectile = null;
            float angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
            float innerAngle;
            float speed = shotSpeed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                innerAngle = angle;

                for (int j = 0; j < timesToShoot; j++)
                {
                    currentProjectile = enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, innerAngle);
                    currentProjectile.transform.DOKill();
                    projectiles.Add(currentProjectile);
                    innerAngle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                StartCoroutine(ClosingBullets(new List<ProjectileBase>(projectiles)));
                projectiles.Clear();

                angle += 2;

                yield return delay;
            }
        }

        private IEnumerator ClosingBullets(List<ProjectileBase> projectiles)
        {
            yield return _closingDelay;

            float endValue;

            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].gameObject.activeSelf)
                {
                    endValue = i % 2 == 0 ? 80 : -80;
                    projectiles[i].Speed = shotSpeedReduction;
                    projectiles[i].transform.DORotate(new Vector3(0, 0, endValue), 1f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
                }
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds innerDelay = new WaitForSeconds(0.1f);
            int timesToFire = timesToShoot / 2;
            float angleStep = 360f / timesToFire;
            float angle = (180 - (angleStep / 2));
            float innerAngle;

            for (int i = 0; i < timesToLoop; i++)
            {
                innerAngle = angle;

                for (int k = 0; k < timesToFire; k++)
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