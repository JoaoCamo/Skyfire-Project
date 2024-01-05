using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Enemy.Attack
{
    public class SingleShot : EnemyAttackBase
    {
        private WaitForSeconds _delay = new WaitForSeconds(0.5f);

        protected override IEnumerator Shoot()
        {
            float speed;

            while (true)
            {
                for (int i = 0; i < timesToShoot; i++)
                {
                    speed = 1;
                    for (int j = 0; j < timesToShoot; j++)
                    {
                        EnemyProjectileManager.RequestAimedBullet?.Invoke(projectileType, transform.position, speed, 1);
                        speed -= 0.1f;
                    }
                    yield return _delay;
                }
            }
        }
    }
}