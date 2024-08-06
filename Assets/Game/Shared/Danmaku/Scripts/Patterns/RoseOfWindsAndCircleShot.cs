using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class RoseOfWindsAndCircleShot : DanmakuBase
    {
        private readonly List<ProjectileBase> projectiles = new List<ProjectileBase>();
        private Coroutine roseOfWindsCoroutine;
        private bool _updateRoseOfWinds = false;

        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.05f);
            WaitForSeconds stopRoseOfWindsDelay = new WaitForSeconds(2f);
            float angle;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            _updateRoseOfWinds = true;

            StartCoroutine(SecondaryShot());

            for (int i = 0; i < timesToLoop; i++)
            {
                StartRoseOfWinds();
                angle = Random.Range(0, 360);

                for (int j = 0; j < 36; j++)
                {
                    for (int k = 0; k < timesToShoot; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, 0.5f, angle));
                        angle += 360f / timesToShoot;
                    }

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return stopRoseOfWindsDelay;

                StopRoseOfWinds();

                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            WaitForSeconds delay = new WaitForSeconds(1f);
            float angle;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                for (int j = 0; j < 18; j++)
                {
                    enemyProjectileManager.FireProjectile(ProjectileType.EnemyOrbBigGreen, transform.position, shotSpeed, angle);
                    angle += 360f / 18;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                yield return delay;
            }
        }

        private void StartRoseOfWinds()
        {
            if(roseOfWindsCoroutine != null)
            {
                StopCoroutine(roseOfWindsCoroutine);
                roseOfWindsCoroutine = null;
            }

            roseOfWindsCoroutine = StartCoroutine(RoseOfWindsSpin());
        }

        private void StopRoseOfWinds()
        {
            if(roseOfWindsCoroutine != null)
            {
                StopCoroutine(roseOfWindsCoroutine);
                roseOfWindsCoroutine = null;
            }

            foreach (ProjectileBase projectile in projectiles)
                projectile.Speed = 0.6f;

            projectiles.Clear();
        }

        private IEnumerator RoseOfWindsSpin()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

            while(_updateRoseOfWinds)
            {
                foreach (ProjectileBase projectile in projectiles)
                    projectile.transform.Rotate(0, 0, 3.5f);

                yield return waitForFixedUpdate;
            }
        }
    }
}