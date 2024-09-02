using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Player.Controls;
using Game.Projectiles;
using Game.Static.Events;
using Game.Static;

namespace Game.Player
{
    public class PlayerAttackBase : MonoBehaviour
    {
        [SerializeField] protected GameObject playerBombPrefab;
        [SerializeField] protected ProjectileType projectileTypePrimary;
        [SerializeField] protected ProjectileType projectileTypeSecondary;

        protected WaitForSeconds _bombCooldown = new WaitForSeconds(5);
        protected ProjectileManager _projectileManager;
        protected PlayerMovement playerMovement;
        protected bool _canAttack = false;
        protected bool _canShoot = true;
        protected int _powerLevel = 0;

        private PlayerHealth _health;

        private PlayerControls _playerControls;
        private InputAction _shootInput;
        private InputAction _bombInput;

        private Coroutine _primaryShotCoroutine;
        private Coroutine _secondaryShotCoroutine;

        private const float MAX_POWER_VALUE = 4;
        private const float MIN_POWER_VALUE = 0;
        private const int MAX_BOMBS = 8;

        private float _currentPower = 0;
        private int _currentBombs = 3;
        private bool _canBomb = true;

        public bool CanShoot { get => _canShoot; set => _canShoot = value; }
        public bool CanBomb { get => _canBomb; set => _canBomb = value; }

        public static Action<int, bool> RequestNewBomb { private set; get; }
        public static Action<float> RequestPowerValueChange { private set; get; }

        private void Awake()
        {
            _projectileManager = GetComponent<ProjectileManager>();
            _health = GetComponent<PlayerHealth>();
            playerMovement = GetComponent<PlayerMovement>();
            
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

            GameInfo.hasUsedBomb = true;
            _currentBombs--;

            _health.RequestInvincibility();
            GameEvents.OnBombValueChange(_currentBombs);
            Drop.DropManager.RequestCollectAll();
            StartCoroutine(FireBombs());
            StartCoroutine(BombCooldown());
        }

        protected virtual IEnumerator FireBombs()
        {
            yield return null;
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

        protected virtual IEnumerator PrimaryShot()
        { 
            yield return null;
        }

        protected virtual IEnumerator SecondaryShot()
        {
            yield return null;
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