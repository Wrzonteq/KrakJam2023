using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerMovementController : MonoBehaviour {
        [SerializeField] CharacterController2D controller;
        [SerializeField] float movmentSpeed = 40f;
        public bool isJumping = false;

        public bool IsMoving { get; private set; }
        public bool IsFacingRight { get; private set; }

        private Vector3 move;
        private InputSystem inputSystem;

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
            controller.Move(move.x * movmentSpeed * Time.fixedDeltaTime, false, isJumping);
            isJumping = false;
        }

        void HandleJump(InputAction.CallbackContext obj) {
            Debug.Log("JUMP");
            isJumping = true;
        }
    }
}
