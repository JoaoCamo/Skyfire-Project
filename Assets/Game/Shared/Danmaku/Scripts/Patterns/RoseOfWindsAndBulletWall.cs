using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class RoseOfWindsAndBulletWall : DanmakuBase
    {
        private readonly List<ProjectileBase> projectiles = new List<ProjectileBase>();
        private Coroutine roseOfWindsCoroutine = null;
        private bool _updateRoseOfWinds = false;

        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.075f);
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            _updateRoseOfWinds = true;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                StartRoseOfWinds();
                angle = Random.Range(0, 360);

                for (int j = 0; j < 50; j++)
                {
                    for (int k = 0; k < timesToShoot; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, shotSpeed, angle));
                        angle += 360f / timesToShoot;
                    }

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                StopRoseOfWinds();

                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds delay = new WaitForSeconds(1.5f);
            WaitForSeconds innerDelay = new WaitForSeconds(0.075f);
            int timesToFire = timesToShoot * 2;
            float angleStep = 360f / timesToFire;
            float angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position, (90 - (angleStep / 2))) : Random.Range(0, 360);
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

        private void StartRoseOfWinds()
        {
            if (roseOfWindsCoroutine != null)
            {
                StopCoroutine(roseOfWindsCoroutine);
                roseOfWindsCoroutine = null;
            }

            roseOfWindsCoroutine = StartCoroutine(RoseOfWindsSpin());
        }

        private void StopRoseOfWinds()
        {
            if (roseOfWindsCoroutine != null)
            {
                StopCoroutine(roseOfWindsCoroutine);
                roseOfWindsCoroutine = null;
            }

            foreach (ProjectileBase projectile in projectiles)
                projectile.Speed = shotSpeedReduction;

            projectiles.Clear();
        }

        private IEnumerator RoseOfWindsSpin()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

            while (_updateRoseOfWinds)
            {
                foreach (ProjectileBase projectile in projectiles)
                    projectile.transform.Rotate(0, 0, 3.5f);

                yield return waitForFixedUpdate;
            }
        }
    }
}