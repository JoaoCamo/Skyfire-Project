using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Projectiles;
using Game.Audio;

namespace Game.Danmaku.Patterns
{
    public class HomingAndScatterCircleShot : DanmakuBase
    {
        private readonly WaitForSeconds _aimAndScatterDelay = new WaitForSeconds(0.75f);

        public override IEnumerator Shoot()
        {
            List<ProjectileBase> projectiles = new List<ProjectileBase>();
            WaitForSeconds delay = new WaitForSeconds(shotDelay);
            WaitForSeconds bulletStopDelay = new WaitForSeconds(0.5f);
            float angle;
            float speed;
            int index;
            bool fireHoaming;

            timesToLoop = isInfiniteLoop ? 999999 : timesToLoop;

            for (int i = 0; i < timesToLoop; i++)
            {
                angle = isAimed ? EnemyProjectileManager.AimAtPlayer(transform.position) : Random.Range(0, 360);
                index = 0;
                fireHoaming = false;

                for (int j = 0; j < 36; j++)
                {
                    speed = shotSpeed;

                    if(index > 2)
                    {
                        fireHoaming = !fireHoaming;
                        index = 0;
                    }

                    for (int k = 0; k < timesToShoot; k++)
                    {
                        projectiles.Add(enemyProjectileManager.GetFireProjectile(fireHoaming ? projectileType : ProjectileType.EnemyBulletBlue, transform.position, speed, angle));
                        speed += shotSpeedReduction;
                    }

                    index++;
                    angle += 360f / 36f;
                }

                SoundEffectController.RequestSfx(SfxTypes.EnemyShoot);

                yield return bulletStopDelay;

                StartCoroutine(AimAndScatter(new List<ProjectileBase>(projectiles)));

                projectiles.Clear();

                yield return delay;
            }
        }

        private IEnumerator AimAndScatter(List<ProjectileBase> projectiles)
        {
            bool isHoaming;
            float angle;

            foreach (ProjectileBase projectile in projectiles)
            {
                DOTween.To(() => projectile.Speed, x => projectile.Speed = x, 0, 0.75f).SetEase(Ease.Linear);
            }

            yield return _aimAndScatterDelay;

            foreach (ProjectileBase projectile in projectiles)
            {
                isHoaming = projectile.ProjectileType == projectileType;
                angle = isHoaming ? EnemyProjectileManager.AimAtPlayer(projectile.transform.position) : Random.Range(0, 360);
                projectile.transform.rotation = Quaternion.Euler(0,0,angle);
                projectile.Speed = isHoaming ? shotSpeed : Random.Range(0.1f, shotSpeed);
            }
        }
    }
}