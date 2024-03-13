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
        private PlayerHealth _health;

        private PlayerControls _playerControls;
        private InputAction _shootInput;
        private InputAction _bombInput;

        private Coroutine _primaryShotCoroutine;
        private Coroutine _secondaryShotCoroutine;
        private readonly WaitForSeconds _primaryShotDelay = new WaitForSeconds(0.075f);
        private readonly WaitForSeconds _secondaryShotDelay = new WaitForSeconds(0.125f);
        private readonly WaitForSeconds _bombDelay = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _bombCooldown = new WaitForSeconds(5);

        private const float MAX_POWER_VALUE = 4;
        private const float MIN_POWER_VALUE = 0;
        private const int MAX_BOMBS = 8;

        private float _currentPower = 0;
        private int _powerLevel = 0;
        private int _currentBombs = 3;
        private bool _canAttack = false;
        private bool _canShoot = true;
        private bool _canBomb = true;

        public bool CanShoot { get => _canShoot; set => _canShoot = value; }

        public static Action<int, bool> RequestNewBomb { private set; get; }
        public static Action<float> RequestPowerValueChange { private set; get; }

        private void Awake()
        {
            _projectileManager = GetComponent<ProjectileManager>();
            _health = GetComponent<PlayerHealth>();
            
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
            ToggleInput(true);
            RequestPowerValueChange += ChangePowerValue;
            RequestNewBomb += AddBomb;
            GameEvents.TogglePlayerInputs += ToggleInput;
        }

        private void OnDisable()
        {
            ToggleInput(false);
            RequestPowerValueChange -= ChangePowerValue;
            RequestNewBomb -= AddBomb;
            GameEvents.TogglePlayerInputs -= ToggleInput;
        }

        private void UseBomb(InputAction.CallbackContext callbackContext)
        {
            if (_currentBombs == 0 || !_canBomb) return;
            
            _currentBombs--;

            _health.RequestInvincibility?.Invoke();
            GameEvents.OnBombValueChange?.Invoke(_currentBombs);
            StartCoroutine(FireBombs());
            StartCoroutine(BombCooldown());
        }

        private IEnumerator FireBombs()
        {
            _canShoot = false;

            for (int i = 0; i < 3; i++)
            {
                Instantiate(playerBombPrefab, transform.position, quaternion.identity);
                yield return _bombDelay;
            }

            _canShoot = true;
        }

        private IEnumerator BombCooldown()
        {
            _canBomb = false;
            yield return _bombCooldown;
            _canBomb = true;
        }

        private void AddBomb(int valueToAdd, bool isReplace)
        {
            if (isReplace)
            {
                _currentBombs = valueToAdd;
                GameEvents.OnBombValueChange(_currentBombs);
                return;
            }

            if (_currentBombs + valueToAdd > MAX_BOMBS)
            {
                _currentBombs = MAX_BOMBS;
                return;
            }

            _currentBombs += valueToAdd;
            GameEvents.OnBombValueChange(_currentBombs);
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

            _canAttack = true;
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

            _canAttack = false;
        }

        private IEnumerator PrimaryShot()
        {
            while (_canAttack)
            {
                if (_canShoot)
                {
                    Vector3 position = transform.position;
                    _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                    _projectileManager.FireProjectile(ProjectileType.PlayerBlue, new Vector3(-0.025f, 0) + position, 2.5f, Quaternion.Euler(0, 0, 0));
                }
                yield return _primaryShotDelay;
            }
        }

        private IEnumerator SecondaryShot()
        {
            while (_canAttack)
            {
                Vector3 position = transform.position;

                if (_canShoot)
                {
                    switch (_powerLevel)
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
                }

                yield return _secondaryShotDelay;
            }
        }

        private void ChangePowerValue(float valueToChange)
        {
            float newPowerValue = _currentPower + valueToChange;
            _currentPower += newPowerValue is <= MAX_POWER_VALUE and >= MIN_POWER_VALUE ? valueToChange : 0;
            _currentPower = newPowerValue > MAX_POWER_VALUE ? MAX_POWER_VALUE : _currentPower;
            _currentPower = newPowerValue < MIN_POWER_VALUE ? MIN_POWER_VALUE : _currentPower;
            _powerLevel = (int)_currentPower >= (_powerLevel + 1) || (int)_currentPower < _powerLevel ? (int)_currentPower : _powerLevel;
            
            GameEvents.OnPowerValueChange?.Invoke(_currentPower);
        }

        private void ToggleInput(bool state)
        {
            if(state)
            {
                _shootInput.Enable();
                _bombInput.Enable();
            }
            else
            {
                _shootInput.Disable();
                _bombInput.Disable();
            }
        }
    }
}