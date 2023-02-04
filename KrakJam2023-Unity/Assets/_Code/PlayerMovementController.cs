using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerMovementController : MonoBehaviour {
        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] int moveForce = 10;
        [SerializeField] int jumpForce = 20;

        Vector3 move;
        InputSystem inputSystem;

        public bool IsMoving { get; private set; }
        public bool IsFacingRight { get; private set; }


        public void Initialise() {
            inputSystem = GameSystems.GetSystem<InputSystem>();
            inputSystem.Bindings.Gameplay.Jump.performed += HandleJump;
        }

        void Update() {
            if (!inputSystem.PlayerInputEnabled) {
                IsMoving = false;
                return;
            }
            UpdateInputValues();

        }
        void UpdateInputValues() {
            move = inputSystem.Bindings.Gameplay.Move.ReadValue<Vector2>();
            IsMoving = Mathf.Abs(move.x) > .5f;
            IsFacingRight = move.x > 0;
        }

        void FixedUpdate() {
            UpdateMovement();
        }

        void UpdateMovement() {
            var moveVector = move * moveForce;
            selfRigidbody2D.AddForce(new Vector2(moveVector.x, 0));
        }
        

        void HandleJump(InputAction.CallbackContext obj) {
            if (!inputSystem.PlayerInputEnabled)
                return;
            Debug.Log($"Jump");
            selfRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
