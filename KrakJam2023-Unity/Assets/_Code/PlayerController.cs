using Cinemachine;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerController : MonoBehaviour {
        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] int movementSpeed = 10;
        [SerializeField] Transform crosshairFollowTarget;
        [SerializeField] CinemachineVirtualCamera playerCamera;
        // [SerializeField] Animator animatorController;
        [SerializeField] GameObject meleChargingAnimation;
        [SerializeField] GameObject rangedChargingAnimation;
        [SerializeField] GameObject avatar;

        float shakeTimer;
        Vector3 move;
        InputSystem inputSystem;
        Transform cachedTransform;

        ChargeableSkillWrapper meleAttack;
        ChargeableSkillWrapper rangedAttack;

        public Transform CrosshairFollowTarget => crosshairFollowTarget;


        public void Initialise() {
            inputSystem = GameSystems.GetSystem<InputSystem>();

            meleAttack = new(inputSystem.Bindings.Gameplay.MeleeAttack, HandleMeleeAttack, HandleStrongMeleeAttack, meleChargingAnimation);
            rangedAttack = new(inputSystem.Bindings.Gameplay.RangedAttack, HandleRangedAttack, HandleStrongRangedAttack, rangedChargingAnimation);
            cachedTransform = transform;
        }

        void HandleMeleeAttack() {

        }

        void HandleStrongMeleeAttack(float chargingDuration) {

        }

        void HandleRangedAttack() {

        }

        void HandleStrongRangedAttack(float chargingDuration) {

        }


        void Update() {
            //if (!inputSystem.PlayerInputEnabled) {
            //    selfRigidbody2D.velocity = Vector2.zero;
            //    // animatorController.SetBool("IsWalking", false);
            //    return;
            //}
            UpdateInputValues();
            UpdateMovement();

            UpdateCamShake();
        }

        void UpdateInputValues() {
            move = inputSystem.Bindings.Gameplay.Move.ReadValue<Vector2>();
            
        }

        void UpdateMovement() {
            var velocity = move * movementSpeed;
            selfRigidbody2D.velocity = velocity;
            var isWalking = velocity.sqrMagnitude > 0.01f;
            // animatorController.SetBool("IsWalking", isWalking);

            if (isWalking) {
                int rot = (move.x < 0) ? 180 : 0;
                avatar.transform.rotation = Quaternion.Euler(0, rot, 0);
            }
        }

        void UpdateCamShake() {
            if (shakeTimer > 0) {
                shakeTimer -= Time.deltaTime;

                if (shakeTimer <= 0) {
                    ShakeCamera(0, 0);
                }
            }
        }

        void Shoot() {
            ShakeCamera(1f, .1f);
        }

        void ShakeCamera(float intensity, float time) {
            var channelPerlin = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            channelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }

        public void Teleport(Vector3 position) {
            var mainCameraTransform = GameSystems.GetSystem<CameraSystem>().MainCamera.transform;
            var playerToCameraOffset = cachedTransform.position - mainCameraTransform.position;
            cachedTransform.position = position;
            mainCameraTransform.position = position - playerToCameraOffset;
        }
    }
}
    
