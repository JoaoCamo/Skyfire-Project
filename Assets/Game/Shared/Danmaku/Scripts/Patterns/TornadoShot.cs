using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class TornadoShot : DanmakuBase
    {
        private bool _canInvertAngle = false;
        private bool _invertAngle = false;
        private const float INVERT_MIN_DELAY = 2.5f;
        private const float INVERT_MAX_DELAY = 5f;

        public override IEnumerator Shoot()
        {
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            _canInvertAngle = true;
            float angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            StartCoroutine(InvertAngleStep());

            for (int i = 0; i < timesToLoop; i++)
            {
                ShotOne(angle);
                ShotTwo(angle);
                ShotThree(angle);

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                angle += _invertAngle ? 10 : -10;

                yield return delay;
            }
        }

        private IEnumerator InvertAngleStep()
        {
            float delay;

            while(_canInvertAngle)
            {
                delay = Random.Range(INVERT_MIN_DELAY, INVERT_MAX_DELAY);

                yield return new WaitForSeconds(delay);
                _invertAngle = !_invertAngle;
            }
        }

        private void ShotOne(float originAngle)
        {
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 10);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 20);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle - 10);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle - 20);
        }

        private void ShotTwo(float originAngle)
        {
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 80);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 90);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 100);

            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle - 80);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle - 90);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle - 100);
        }

        private void ShotThree(float originAngle)
        {
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 120);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 155);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 175);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 180);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 185);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 205);
            enemyProjectileManager.FireProjectile(projectileType, transform.position, shotSpeed, originAngle + 240);
        }
    }
}