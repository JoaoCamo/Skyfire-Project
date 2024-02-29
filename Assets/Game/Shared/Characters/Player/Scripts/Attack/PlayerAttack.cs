using System;
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

        private Coroutine _primaryShotCoroutine;
        private Coroutine _secondaryShotCoroutine;
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(0.075f);
        private readonly WaitForSeconds _secondaryShotDelay = new WaitForSeconds(0.125f);

        private const float MAX_POWER_VALUE = 4;
        private const float MIN_POWER_VALUE = 0;

        private float _currentPower = 0;
        private int _powerLevel = 0;
        private bool _canShoot = false;

        public static Action<float> RequestPowerValueChange { private set; get; }

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
            RequestPowerValueChange += ChangePowerValue;
        }

        private void OnDisable()
        {
            _shootInput.Disable();
            RequestPowerValueChange -= ChangePowerValue;
        }

        private void StartShot(InputAction.CallbackContext callbackContext)
        {
            if (_primaryShotCoroutine != null)
            {
                StopCoroutine(_primaryShotCoroutine);
                _primaryShotCoroutine = null;
            }

            if (_secondaryShotCoroutine != null)
            {
                StopCoroutine(_secondaryShotCoroutine);
                _secondaryShotCoroutine = null;
            }

            _canShoot = true;
            _primaryShotCoroutine = StartCoroutine(PrimaryShot());
            _secondaryShotCoroutine = StartCoroutine(SecondaryShot());
        }

        private void StopShot(InputAction.CallbackContext callbackContext)
        {
            if (_primaryShotCoroutine != null)
            {
                StopCoroutine(_primaryShotCoroutine);
                _primaryShotCoroutine = null;
            }

            if(_secondaryShotCoroutine != null)
            {
                StopCoroutine(_secondaryShotCoroutine);
                _secondaryShotCoroutine = null;
            }

            _canShoot = false;
        }

        private IEnumerator PrimaryShot()
        {
            while (_canShoot)
            {
                Vector3 position = transform.position;
                _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(0.025f, 0) + position, 2.5f, 1, Quaternion.Euler(0, 0, 0));
                _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(-0.025f, 0) + position, 2.5f, 1, Quaternion.Euler(0, 0, 0));
                yield return _primaryShotDelay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            while (_canShoot)
            {
                switch(_powerLevel)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }

                yield return _secondaryShotDelay;
            }
        }

        private void ChangePowerValue(float valueToChange)
        {
            float newPowerValue = _currentPower + valueToChange;
            _currentPower += (newPowerValue <= MAX_POWER_VALUE && newPowerValue >= MIN_POWER_VALUE) ? valueToChange : 0;
            _currentPower = _currentPower > MAX_POWER_VALUE ? MAX_POWER_VALUE : _currentPower;
            _powerLevel = (int)_currentPower >= (_powerLevel + 1) || (int)_currentPower < _powerLevel ? (int)_currentPower : _powerLevel;
        }
    }
}