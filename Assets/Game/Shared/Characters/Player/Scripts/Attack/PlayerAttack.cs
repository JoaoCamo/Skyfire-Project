using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Game.Player.Controls;
using Game.Projectiles;
using Game.Static.Events;

namespace Game.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject playerBombPrefab;
        
        private ProjectileManager _projectileManager;

        private PlayerControls _playerControls;
        private InputAction _shootInput;
        private InputAction _bombInput;

        private Coroutine _primaryShotCoroutine;
        private Coroutine _secondaryShotCoroutine;
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(0.075f);
        private readonly WaitForSeconds _secondaryShotDelay = new WaitForSeconds(0.125f);
        private readonly WaitForSeconds _bombCooldown = new WaitForSeconds(5);

        private const float MAX_POWER_VALUE = 4;
        private const float MIN_POWER_VALUE = 0;
        private const int MAX_BOMBS = 8;

        private float _currentPower = 0;
        private int _powerLevel = 0;
        private int _currentBombs = 3;
        private bool _canShoot = false;
        private bool _canBomb = true;

        public int CurrentBombs { get => _currentBombs; set => _currentBombs = value; }
        
        public static Action<float> RequestPowerValueChange { private set; get; }

        private void Awake()
        {
            _projectileManager = GetComponent<ProjectileManager>();
            
            _playerControls = new PlayerControls();
            _shootInput = _playerControls.Player.Shoot;
            _bombInput = _playerControls.Player.Bomb;
            
            _shootInput.performed += StartShot;
            _shootInput.canceled += StopShot;

            _bombInput.performed += UseBomb;
            
            GameEvents.OnBombValueChange?.Invoke(_currentBombs);
        }

        private void OnEnable()
        {
            _shootInput.Enable();
            _bombInput.Enable();
            RequestPowerValueChange += ChangePowerValue;
        }

        private void OnDisable()
        {
            _shootInput.Disable();
            _bombInput.Disable();
            RequestPowerValueChange -= ChangePowerValue;
        }

        private void UseBomb(InputAction.CallbackContext callbackContext)
        {
            if (_currentBombs == 0 || !_canBomb) return;

            Instantiate(playerBombPrefab, transform.position, quaternion.identity);
            _currentBombs--;
            
            GameEvents.OnBombValueChange?.Invoke(_currentBombs);
            StartCoroutine(BombCooldown());
        }

        private IEnumerator BombCooldown()
        {
            _canBomb = false;
            yield return _bombCooldown;
            _canBomb = true;
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
                _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(-0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                yield return _primaryShotDelay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            while (_canShoot)
            {
                Vector3 position = transform.position;
                
                switch(_powerLevel)
                {
                    case 1:
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(0, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        break;
                    case 2:
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        break;
                    case 3:
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(0f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        break;
                    case 4:
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(-0.075f, 0f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        _projectileManager.FireProjectile(ProjectileType.PlayerMissile, new Vector3(-0.05f, 0.1f) + position, 2f, Quaternion.Euler(0, 0, 0));
                        break;
                }

                yield return _secondaryShotDelay;
            }
        }

        private void ChangePowerValue(float valueToChange)
        {
            float newPowerValue = _currentPower + valueToChange;
            _currentPower += newPowerValue is <= MAX_POWER_VALUE and >= MIN_POWER_VALUE ? valueToChange : 0;
            _currentPower = newPowerValue > MAX_POWER_VALUE ? MAX_POWER_VALUE : _currentPower;
            _powerLevel = (int)_currentPower >= (_powerLevel + 1) || (int)_currentPower < _powerLevel ? (int)_currentPower : _powerLevel;
            
            GameEvents.OnPowerValueChange?.Invoke(_currentPower);
        }
    }
}