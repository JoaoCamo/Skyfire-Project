using UnityEngine;
using UnityEngine.InputSystem;
using Game.Player.Controls;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseMoveSpeed;
        
        private Rigidbody2D _rigidbody2D;
        private PlayerControls _playerControls;
        private InputAction _moveControl;
        private Vector2 _moveInput;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _playerControls = new PlayerControls();
            _moveControl = _playerControls.Player.Move;
        }

        private void OnEnable()
        {
            _moveControl.Enable();
        }

        private void OnDisable()
        {
            _moveControl.Disable();
        }

        private void Update()
        {
            _moveInput = _moveControl.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = _moveInput * (baseMoveSpeed);
        }
    }
}