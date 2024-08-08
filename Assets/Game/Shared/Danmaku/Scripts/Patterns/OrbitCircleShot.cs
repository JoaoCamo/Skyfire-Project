using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class OrbitCircleShot : DanmakuBase
    {
        private readonly List<ProjectileBase> projectiles = new List<ProjectileBase>();
        private Coroutine _orbitCoroutine;
        private bool _orbitDirection = true;
        private bool _canOrbit = false;

        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds innerDelay = new WaitForSeconds(0.1f);
            WaitForSeconds stopOrbitDelay = new WaitForSeconds(1.5f);
            float angle;
            float speed;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;
            _canOrbit = true;

            for (int i = 0; i < timesToLoop; i++)
            {
                StartOrbit();
                speed = shotSpeed;

                for (int j = 0; j < 6; j++)
                {
                    angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(projectileType, transform.position, speed, angle));
                        angle += 360f / timesToShoot;
                    }

                    speed += shotSpeedReduction;

                    SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);
                    yield return innerDelay;
                }

                yield return stopOrbitDelay;

                StopOrbit();

                _orbitDirection = !_orbitDirection;

                yield return delay;
            }
        }

        private void StartOrbit()
        {
            if (_orbitCoroutine != null)
            {
                StopCoroutine(_orbitCoroutine);
                _orbitCoroutine = null;
            }

            _orbitCoroutine = StartCoroutine(Orbit());
        }

        private void StopOrbit()
        {
            if (_orbitCoroutine != null)
            {
                StopCoroutine(_orbitCoroutine);
                _orbitCoroutine = null;
            }

            projectiles.Clear();
        }    

        private IEnumerator Orbit()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            float angleStep;

            while (_canOrbit)
            {
                angleStep = _orbitDirection ? 1f : -1f;

                foreach (ProjectileBase projectile in projectiles)
                    projectile.transform.Rotate(0, 0, angleStep);

                yield return waitForFixedUpdate;
            }
        }
    }
}