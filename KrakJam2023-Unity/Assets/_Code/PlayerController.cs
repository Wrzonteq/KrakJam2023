using Cinemachine;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerController : MonoBehaviour {
        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] int movementSpeed = 10;
        [SerializeField] Transform crosshairFollowTarget;
        [SerializeField] CinemachineVirtualCamera playerCamera;
        [SerializeField] Animator animatorController;
        [SerializeField] GameObject meleChargingAnimation;
        [SerializeField] GameObject rangedChargingAnimation;
        [SerializeField] GameObject avatar;

        [SerializeField] float attackDuration;

        bool isAttacking;
        float inputUnlockTime;
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
            inputUnlockTime = Time.time + attackDuration;
            //todo find enemies in range and deal damage
            animatorController.SetTrigger("AttackMelee");
        }

        void HandleStrongMeleeAttack(float chargingDuration) {
            animatorController.SetBool("IsChargingMelee", false);
            animatorController.SetTrigger("AttackMelee");
        }

        void HandleRangedAttack() {
            inputUnlockTime = Time.time + attackDuration;
            //todo find enemies in range and deal damage
            animatorController.SetTrigger("AttackRange");
        }

        void HandleStrongRangedAttack(float chargingDuration) {
            animatorController.SetBool("IsChargingRanged", false);
            animatorController.SetTrigger("AttackRange");
        }


        void Update() {
            if (!inputSystem.PlayerInputEnabled) {
                animatorController.SetBool("IsWalking", false);
                return;
            }
            UpdateInputValues();
            if (inputUnlockTime > Time.time)
                return;
            UpdateMovement();
            UpdateChargingAnimations();
        }

        void UpdateChargingAnimations() {
            animatorController.SetBool("IsChargingMelee", meleAttack.IsCharging);
            animatorController.SetBool("IsChargingRanged", rangedAttack.IsCharging);
        }

        void UpdateInputValues() {
            move = inputSystem.Bindings.Gameplay.Move.ReadValue<Vector2>();
            
        }

        void UpdateMovement() {
            var velocity = move * movementSpeed;
            selfRigidbody2D.velocity = velocity;
            var isWalking = velocity.sqrMagnitude > 0.01f;
            animatorController.SetBool("IsWalking", isWalking);

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
    
