using Cinemachine;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerController : MonoBehaviour {
        [SerializeField] CinemachineVirtualCamera playerCamera;
        [SerializeField] Animator animatorController;
        [SerializeField] ParticleSystem meleChargingAnimation;
        [SerializeField] ParticleSystem rangedChargingAnimation;
        [SerializeField] GameObject avatar;
        [SerializeField] PlayerMovementController movement;
        [SerializeField] StaffLightningController staffController;
        [SerializeField] PlayerAnimationEventBroadcaster animationEvents;
        [SerializeField] float attackDuration;

        bool isAttacking;
        float inputUnlockTime;
        InputSystem inputSystem;
        Transform cachedTransform;

        ChargeableSkillWrapper meleAttack;
        ChargeableSkillWrapper rangedAttack;


        public void Initialise() {
            inputSystem = GameSystems.GetSystem<InputSystem>();
            movement.Initialise();

            meleAttack = new(inputSystem.Bindings.Gameplay.MeleeAttack, HandleMeleeAttack, HandleStrongMeleeAttack, meleChargingAnimation);
            rangedAttack = new(inputSystem.Bindings.Gameplay.RangedAttack, HandleRangedAttack, HandleStrongRangedAttack, rangedChargingAnimation);
            cachedTransform = transform;
            animationEvents.callBackOnBoink = OnBoink;
            animationEvents.callBackOnFire = OnFire;
        }

        void OnFire() {
            var enemy = FindObjectOfType<Enemy>();
            if (enemy != null)
                staffController.ShootTarget(enemy.transform);
            else
                Debug.LogError($"Enemy not found");
        }

        void OnBoink() {
            //todo find enemies in range and deal damage
        }

        void HandleMeleeAttack() {
            inputUnlockTime = Time.time + attackDuration;
            animatorController.SetTrigger("AttackMelee");
        }

        void HandleStrongMeleeAttack(float chargingDuration) {
            animatorController.SetBool("IsChargingMelee", false);
            HandleMeleeAttack();
        }

        void HandleRangedAttack() {
            inputUnlockTime = Time.time + attackDuration;
            animatorController.SetTrigger("AttackRange");
        }

        void HandleStrongRangedAttack(float chargingDuration) {
            animatorController.SetBool("IsChargingRanged", false);
            HandleRangedAttack();
        }


        void Update() {
            if (!inputSystem.PlayerInputEnabled) {
                return;
            }

            animatorController.SetBool("IsWalking", movement.IsMoving);
            UpdateVisuals();
            // if (inputUnlockTime > Time.time)
                // return;
            UpdateChargingAnimations();
        }

        void UpdateChargingAnimations() {
            animatorController.SetBool("IsChargingMelee", meleAttack.IsCharging);
            animatorController.SetBool("IsChargingRanged", rangedAttack.IsCharging);
        }

        void UpdateVisuals() {
            animatorController.SetBool("IsWalking", movement.IsMoving);

            if (movement.IsMoving) {
                var rot = movement.IsFacingRight ? 0 : 180;
                avatar.transform.rotation = Quaternion.Euler(0, rot, 0);
            }
        }

        public void Teleport(Vector3 position) {
            var mainCameraTransform = GameSystems.GetSystem<CameraSystem>().MainCamera.transform;
            var playerToCameraOffset = cachedTransform.position - mainCameraTransform.position;
            cachedTransform.position = position;
            mainCameraTransform.position = position - playerToCameraOffset;
        }
    }
}
    
