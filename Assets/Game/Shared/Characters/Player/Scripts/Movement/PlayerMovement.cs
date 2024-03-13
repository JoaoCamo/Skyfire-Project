using UnityEngine;
using UnityEngine.InputSystem;
using Game.Player.Controls;
using Game.Static.Events;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseMoveSpeed;
        [SerializeField] private Transform focusCollectArea;
        [SerializeField] private GameObject hitBoxSprite;
        
        private float _moveSpeed;
        private float _speedMultiplayer = 1;
        private Rigidbody2D _rigidbody2D;
        private PlayerControls _playerControls;
        private InputAction _moveControl;
        private InputAction _focusControl;
        private Vector2 _moveInput;
        private bool _isUsingFocus = false;

        private readonly Vector2 _collectAreaNormalScale = new Vector2(1, 1);
        private readonly Vector2 _collectAreaFocusScale = new Vector2(1.5f, 1.5f);

        public float SpeedMultiplayer { get => _speedMultiplayer; set => _speedMultiplayer = value; }
        public static Transform PlayerTransform;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _playerControls = new PlayerControls();
            _moveControl = _playerControls.Player.Move;
            _focusControl = _playerControls.Player.Focus;

            _focusControl.performed += ToggleFocus;
            _focusControl.canceled += ToggleFocus;

            _moveSpeed = baseMoveSpeed;
            PlayerTransform = transform;
        }

        private void OnEnable()
        {
            ToggleInput(true);
            GameEvents.TogglePlayerInputs += ToggleInput;
        }

        private void OnDisable()
        {
            ToggleInput(false);
            GameEvents.TogglePlayerInputs -= ToggleInput;
        }

        private void Update()
        {
            _moveInput = _moveControl.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = _moveInput * (_moveSpeed * _speedMultiplayer);
        }

        private void ToggleFocus(InputAction.CallbackContext context)
        {
            _isUsingFocus = !_isUsingFocus;

            hitBoxSprite.SetActive(_isUsingFocus);
            focusCollectArea.localScale = _isUsingFocus ? _collectAreaFocusScale : _collectAreaNormalScale;
            _moveSpeed = _isUsingFocus ? baseMoveSpeed / 2f : baseMoveSpeed;
        }

        private void ToggleInput(bool state)
        {
            if (state)
            {
                _moveControl.Enable();
                _focusControl.Enable();
            }
            else
            {
                _moveControl.Disable();
                _focusControl.Disable();
            }
        }
    }
}