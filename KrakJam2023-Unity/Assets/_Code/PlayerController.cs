using Cinemachine;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public enum AttackType { Melee = 0, Ranged = 1 }

    public class PlayerController : Creature {
        [SerializeField] CinemachineVirtualCamera playerCamera;
        [SerializeField] Animator animatorController;
        [SerializeField] GameObject avatar;
        [SerializeField] PlayerMovementController movement;
        [SerializeField] StaffLightningController staffController;
        [SerializeField] PlayerAnimationEventBroadcaster animationEvents;
        [SerializeField] float attackDuration;
        [SerializeField] float meleeRange;
        [SerializeField] float maxRange;
        [SerializeField] float aimConeAngle = 30f;
        [SerializeField] ChargeableSkillWrapper meleAttack;
        [SerializeField] ChargeableSkillWrapper rangedAttack;

        bool attackAnimationInProgress;
        InputSystem inputSystem;
        Transform cachedTransform;

        public CinemachineVirtualCamera Camera => playerCamera;
        Vector3 ForwardVector => transform.localScale.x > 0 ? transform.right : -transform.right;
        public Vector3 Position => cachedTransform.position;


        public void Initialise() {
            inputSystem = GameSystems.GetSystem<InputSystem>();
            movement.Initialise();
            meleAttack.Init();
            rangedAttack.Init();
            meleAttack.StartedEvent += HandleAttackStarted;
            rangedAttack.StartedEvent += HandleAttackStarted;

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
            var pointAtHalfMeleeDistanceInFront = transform.position + ForwardVector *meleeRange/2;
            var hits = Physics2D.CircleCastAll(pointAtHalfMeleeDistanceInFront, meleeRange/2f, ForwardVector, meleeRange);
            foreach (var h in hits) {
                var isEnemy = h.transform.CompareTag("Enemy");
                if (isEnemy) {
                    var enemy = h.transform.GetComponent<Enemy>();
                    enemy.DealDamage(meleAttack.Damage);
                    Debug.Log($"Deal {meleAttack.Damage} to {enemy.gameObject.name}. Distance to target {(enemy.transform.position - Position).magnitude}");
                }
            }
        }

        void OnFire() {
            var hits = Physics2D.CircleCastAll(transform.position, 1.5f, ForwardVector, maxRange);
            Debug.Log($"Hit {hits.Length} targets");
            foreach (var h in hits) {
                var distanceVector = h.transform.position - transform.position;
                var absoluteAngle = Vector3.Angle(distanceVector.normalized, ForwardVector);
                if(absoluteAngle > aimConeAngle)
                    continue;
                var isEnemy = h.transform.CompareTag("Enemy");
                if (isEnemy) {
                    var enemy = h.transform.GetComponent<Enemy>();
                    staffController.ShootTarget(enemy.transform, rangedAttack.CurrentAttackIsStronk);
                    enemy.DealDamage(rangedAttack.Damage);
                    Debug.Log($"Deal {rangedAttack.Damage} to {enemy.gameObject.name}. Distance to target {(enemy.transform.position - Position).magnitude}");
                    break;
                }
                Debug.Log($"No enemies found!");
            }
        }

        void Update() {
            if (!inputSystem.PlayerInputEnabled || IsInputLocked())
                return;
            UpdateVisuals();
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

        void OnDestroy() {
            meleAttack.Uninit();
            rangedAttack.Uninit();
        }
    }
}
