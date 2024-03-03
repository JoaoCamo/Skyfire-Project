using UnityEngine;
using UnityEngine.InputSystem;
using Game.Player.Controls;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseMoveSpeed;
        [SerializeField] private GameObject focusCollectArea;
        [SerializeField] private GameObject hitBoxSprite;
        
        private float _moveSpeed;
        private float _speedMultiplayer = 1;
        private Rigidbody2D _rigidbody2D;
        private PlayerControls _playerControls;
        private InputAction _moveControl;
        private InputAction _focusControl;
        private Vector2 _moveInput;
        
        public float SpeedMultiplayer { get => _speedMultiplayer; set => _speedMultiplayer = value; }
        public static Transform PlayerTransform;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _playerControls = new PlayerControls();
            _moveControl = _playerControls.Player.Move;
            _focusControl = _playerControls.Player.Focus;

            _focusControl.performed += ToggleFocusOn;
            _focusControl.canceled += ToggleFocusOff;

            _moveSpeed = baseMoveSpeed;
            PlayerTransform = transform;
        }

        private void OnEnable()
        {
            _moveControl.Enable();
            _focusControl.Enable();
        }

        private void OnDisable()
        {
            _moveControl.Disable();
            _focusControl.Disable();
        }

        private void Update()
        {
            _moveInput = _moveControl.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = _moveInput * (_moveSpeed * _speedMultiplayer);
        }

        private void ToggleFocusOn(InputAction.CallbackContext context)
        {
            hitBoxSprite.SetActive(true);
            focusCollectArea.SetActive(true);
            _moveSpeed = baseMoveSpeed / 2f;
        }
        
        private void ToggleFocusOff(InputAction.CallbackContext context)
        {
            focusCollectArea.SetActive(false);
            hitBoxSprite.SetActive(false);
            _moveSpeed = baseMoveSpeed;
        }
    }
}