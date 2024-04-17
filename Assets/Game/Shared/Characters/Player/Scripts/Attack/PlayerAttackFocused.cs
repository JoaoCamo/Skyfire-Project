using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public class PlayerAttackFocused : PlayerAttackBase
    {
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(0.075f);
        private readonly WaitForSeconds _secondaryShotDelay = new WaitForSeconds(0.125f);

        protected override IEnumerator PrimaryShot()
        {
            while (_canAttack)
            {
                if (_canShoot)
                {
                    Vector3 position = transform.position;
                    _projectileManager.FireProjectile(projectileTypePrimary, new Vector3(0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                    _projectileManager.FireProjectile(projectileTypePrimary, new Vector3(-0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                }
                yield return _primaryShotDelay;
            }
        }

        protected override IEnumerator SecondaryShot()
        {
            while (_canAttack)
            {
                Vector3 position = transform.position;

                if (_canShoot)
                {
                    switch (_powerLevel)
                    {
                        case 1:
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            break;
                        case 2:
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            break;
                        case 3:
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            break;
                        case 4:
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                            break;
                    }
                }

                yield return _secondaryShotDelay;
            }
        }
    }
}