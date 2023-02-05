using Cinemachine;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerController : Creature {
        [SerializeField] CinemachineVirtualCamera playerCamera;
        [SerializeField] Animator animatorController;
        [SerializeField] ParticleSystem meleChargingAnimation;
        [SerializeField] ParticleSystem rangedChargingAnimation;
        [SerializeField] GameObject avatar;
        [SerializeField] PlayerMovementController movement;
        [SerializeField] StaffLightningController staffController;
        [SerializeField] PlayerAnimationEventBroadcaster animationEvents;
        [SerializeField] float attackDuration;
        [SerializeField] float meleeRange;

        bool attackAnimationInProgress;

        InputSystem inputSystem;
        Transform cachedTransform;

        ChargeableSkillWrapper meleAttack;
        ChargeableSkillWrapper rangedAttack;


        public void Initialise() {
            inputSystem = GameSystems.GetSystem<InputSystem>();
            movement.Initialise();

            meleAttack = new(
                inputSystem.Bindings.Gameplay.MeleeAttack,
                HandleAttackStarted,
                HandleMeleeAttack,
                HandleStrongMeleeAttack,
                meleChargingAnimation);
            rangedAttack = new(
                inputSystem.Bindings.Gameplay.RangedAttack,
                HandleAttackStarted,
                HandleRangedAttack,
                HandleStrongRangedAttack,
                rangedChargingAnimation);

            cachedTransform = transform;
            animationEvents.callBackOnBoink = OnBoink;
            animationEvents.callBackOnFire = OnFire;
            PlayerIdleStateBehaviour.IdleStateEntered += HandleIdleStateEntered;
        }

        void HandleAttackStarted() {
            attackAnimationInProgress = true;
            Debug.Log($"Attack is now in progress!");
        }

        void HandleIdleStateEntered() {
            attackAnimationInProgress = false;
        }

        void OnBoink() {
            var hits = Physics2D.CircleCastAll(transform.position, meleeRange, transform.forward);
            foreach (var h in hits) {
                var isEnemy = h.transform.CompareTag("Enemy");
                if (isEnemy) {
                    var enemy = h.transform.GetComponent<Enemy>();
                    enemy.DealDamage(meleAttack.CurrentAttackIsStronk ? 3 : 1);
                }
            }
            //todo find enemies in range and deal damage
        }

        void OnFire() {
            var enemy = FindObjectOfType<Enemy>();
            if (enemy != null) {
                staffController.ShootTarget(enemy.transform, rangedAttack.CurrentAttackIsStronk);
                enemy.DealDamage(rangedAttack.CurrentAttackIsStronk ? 3 : 1);
            } else
                Debug.LogError($"Enemy not found");
        }

        void HandleMeleeAttack() {
            animatorController.SetTrigger("AttackMelee");
        }

        void HandleStrongMeleeAttack(float chargingDuration) {
            animatorController.SetBool("IsChargingMelee", false);
            HandleMeleeAttack();
        }

        void HandleRangedAttack() {
            animatorController.SetTrigger("AttackRange");
        }

        void HandleStrongRangedAttack(float chargingDuration) {
            animatorController.SetBool("IsChargingRanged", false);
            HandleRangedAttack();
        }


        void Update() {
            if (!inputSystem.PlayerInputEnabled || IsInputLocked()) {
                return;
            }
            UpdateVisuals();
            UpdateChargingAnimations();
        }

        void UpdateChargingAnimations() {
            animatorController.SetBool("IsChargingMelee", meleAttack.IsCharging);
            animatorController.SetBool("IsChargingRanged", rangedAttack.IsCharging);
        }

        void UpdateVisuals() {
            animatorController.SetBool("IsWalking", movement.IsMoving);
        }

        public void Teleport(Vector3 position) {
            var mainCameraTransform = GameSystems.GetSystem<CameraSystem>().MainCamera.transform;
            var playerToCameraOffset = cachedTransform.position - mainCameraTransform.position;
            cachedTransform.position = position;
            mainCameraTransform.position = position - playerToCameraOffset;
        }

        protected override void Die() {
            PlayerIdleStateBehaviour.IdleStateEntered -= HandleIdleStateEntered;
            inputSystem.DisableInput();
            animatorController.SetBool("IsDead", true);

            //todo load gameover screen, switch to interface input
        }

        public bool IsInputLocked() {
            return attackAnimationInProgress;
        }
    }
}
    
