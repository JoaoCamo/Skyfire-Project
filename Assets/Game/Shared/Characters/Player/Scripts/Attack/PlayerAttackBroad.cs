using System.Collections;
using UnityEngine;
using Game.Audio;

namespace Game.Player
{
    public class PlayerAttackBroad : PlayerAttackBase
    {
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(0.075f);
        private readonly WaitForSeconds _secondaryShotDelay = new WaitForSeconds(0.1f);
        private readonly WaitForSeconds _bombDelay = new WaitForSeconds(0.5f);

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
                if (_canShoot)
                {
                    switch (playerMovement.IsUsingFocus)
                    {
                        case true:
                            SecondaryShotFocus();
                            break;
                        case false:
                            SecondaryShotNormal();
                            break;
                    }
                }
                yield return _secondaryShotDelay;
            }
        }

        protected override IEnumerator FireBombs()
        {
            _canShoot = false;

            for (int i = 0; i < 4; i++)
            {
                Instantiate(playerBombPrefab, transform.position, Quaternion.identity);
                SoundEffectController.RequestSfx?.Invoke(SfxTypes.PlayerShoot);
                yield return _bombDelay;
            }

            _canShoot = true;
        }

        private void SecondaryShotNormal()
        {
            Vector3 position = transform.position;
            switch (_powerLevel)
            {
                case 1:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                    break;
                case 2:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, -10));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 10));
                    break;
                case 3:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, -10));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 10));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                    break;
                case 4:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, -10));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 10));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, -2.5f));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 2.5f));
                    break;
            }
            SoundEffectController.RequestSfx?.Invoke(SfxTypes.PlayerShoot);
        }

        private void SecondaryShotFocus()
        {
            Vector3 position = transform.position;
            switch (_powerLevel)
            {
                case 1:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                    break;
                case 2:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, -5));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 5));
                    break;
                case 3:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, -5));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 5));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                    break;
                case 4:
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, -5));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 5));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 2.5f));
                    _projectileManager.FireProjectile(projectileTypeSecondary, new Vector3(-0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, -2.5f));
                    break;
            }
            SoundEffectController.RequestSfx?.Invoke(SfxTypes.PlayerShoot);
        }
    }
}