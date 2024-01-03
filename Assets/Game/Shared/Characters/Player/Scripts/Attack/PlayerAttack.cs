using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Player.Controls;
using Game.Projectiles;

namespace Game.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private ProjectileManager _projectileManager;

        private PlayerControls _playerControls;
        private InputAction _shootInput;

        private Coroutine _shootCoroutine;
        private readonly WaitForSeconds _shotDelay = new WaitForSeconds(0.075f);

        private bool _canShoot = false;

        private void Awake()
        {
            _projectileManager = GetComponent<ProjectileManager>();
            
            _playerControls = new PlayerControls();
            _shootInput = _playerControls.Player.Shoot;
            _shootInput.performed += StartShot;
            _shootInput.canceled += StopShot;
        }

        private void OnEnable()
        {
            _shootInput.Enable();
        }

        private void OnDisable()
        {
            _shootInput.Disable();
        }

        private void StartShot(InputAction.CallbackContext callbackContext)
        {
            _canShoot = true;
            _shootCoroutine = StartCoroutine(Shoot());
        }

        private void StopShot(InputAction.CallbackContext callbackContext)
        {
            if (_shootCoroutine != null)
            {
                StopCoroutine(_shootCoroutine);
                _shootCoroutine = null;
            }

            _canShoot = false;
        }

        private IEnumerator Shoot()
        {
            while (_canShoot)
            {
                Vector3 position = transform.position;
                _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(0.025f, 0) + position, 2.5f, 1, Quaternion.Euler(0, 0, 0));
                _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(-0.025f, 0) + position, 2.5f, 1, Quaternion.Euler(0, 0, 0));
                yield return _shotDelay;
            }
        }
    }
}