using System.Collections;
using UnityEngine;
using Game.Audio;

namespace Game.Player
{
    public class PlayerAttackFocused : PlayerAttackBase
    {
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(0.075f);
        private readonly WaitForSeconds _secondaryShotDelay = new WaitForSeconds(0.125f);
        protected new WaitForSeconds _bombCooldown = new WaitForSeconds(10);

        protected override IEnumerator PrimaryShot()
        {
            while (_canAttack)
            {
                if (_canShoot)
                {
                    Vector3 position = transform.position;
                    _projectileManager.FireProjectile(projectileTypePrimary, new Vector3(0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                    _projectileManager.FireProjectile(projectileTypePrimary, new Vector3(-0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                    SoundEffectController.RequestSfx?.Invoke(SfxTypes.PlayerShoot);
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
                    SoundEffectController.RequestSfx?.Invoke(SfxTypes.PlayerShoot);
                }
                yield return _secondaryShotDelay;
            }
        }

        protected override IEnumerator FireBombs()
        {
            _canShoot = false;

            SoundEffectController.RequestSfx?.Invoke(SfxTypes.PlayerShoot);
            PlayerBombBeam bomb =  Instantiate(playerBombPrefab, transform).GetComponent<PlayerBombBeam>();

            yield return StartCoroutine(bomb.StartBeam());

            _canShoot = true;
        }
    }
}