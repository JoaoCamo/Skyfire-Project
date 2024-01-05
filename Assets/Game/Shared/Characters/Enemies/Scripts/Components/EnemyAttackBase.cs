using System.Collections;
using UnityEngine;
using Game.Projectiles;

namespace Game.Enemy
{
    public class EnemyAttackBase : MonoBehaviour
    {
        protected ProjectileType projectileType;
        protected bool isInfiniteLoop;
        protected int timesToLoop;
        protected int timesToShoot;

        public void StartShot()
        {
            StartCoroutine(Shoot());
        }

        public void SetShot(ProjectileType projectileType, bool isInfiniteLoop, int timesToLoop, int timesToShoot)
        {
            this.projectileType = projectileType;
            this.isInfiniteLoop = isInfiniteLoop;
            this.timesToLoop = timesToLoop;
            this.timesToShoot = timesToShoot;
        }

        protected virtual IEnumerator Shoot()
        {
            yield return null;
        }
    }
}