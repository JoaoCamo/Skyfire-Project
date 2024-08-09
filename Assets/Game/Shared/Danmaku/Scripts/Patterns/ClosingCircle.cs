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
            float angle;
            float speed = shotSpeed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < timesToShoot; j++)
                {
                    currentProjectile = enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle);
                    currentProjectile.transform.DOKill();
                    projectiles.Add(currentProjectile);
                    angle += 360f / timesToShoot;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                StartCoroutine(ClosingBullets(new List<ProjectileBase>(projectiles)));
                projectiles.Clear();

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
            WaitForSeconds delay = new WaitForSeconds(1.5f);
            WaitForSeconds innerDelay = new WaitForSeconds(0.1f);
            float angle;
            float innerAngle;
            int timesToFire = timesToShoot / 2;

            for (int i = 0; i < timesToLoop; i++)
            {
                yield return delay;

                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, 60) : Random.Range(0, 360);

                for (int j = 0; j < 60; j++)
                {
                    innerAngle = angle;

                    for (int k = 0; k < timesToFire; k++)
                    {
                        enemyProjectileManager.GetFireProjectile(ProjectileType.EnemyBulletTwoBlue, transform.position, 1.25f, innerAngle);
                        innerAngle += 20;
                    }

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                    yield return innerDelay;
                }
            }
        }
    }
}