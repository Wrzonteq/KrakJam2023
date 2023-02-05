using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PartTimeKamikaze.KrakJam2023 {
    [Serializable]
    public class ChargeableSkillWrapper {
        public event Action StartedEvent;

        [SerializeField] PlayerController player;
        [SerializeField] Animator animator;
        [SerializeField] AttackType attackType;
        [SerializeField] ParticleSystem chargingVisuals;
        [SerializeField] int normalDamage;
        [SerializeField] int strongDamage;

        int startedActionsCount;

        public bool IsCharging { get; private set; }
        public bool CurrentAttackIsStronk { get; private set; }
        public int Damage => CurrentAttackIsStronk ? strongDamage : normalDamage;


        public void Init() {
            var action2 = attackType == AttackType.Melee ? GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.MeleeAttack : GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.RangedAttack;
            action2.started += OnStarted;
            action2.performed += OnPerformed;
            action2.canceled += OnCanceled;
        }

        public void Uninit() {
            var action2 = attackType == AttackType.Melee ? GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.MeleeAttack : GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.RangedAttack;
            action2.started -= OnStarted;
            action2.performed -= OnPerformed;
            action2.canceled -= OnCanceled;
        }

        void OnStarted(InputAction.CallbackContext context) {
            Debug.Log($"{attackType} OnStarted");
            if (player.IsInputLocked() && startedActionsCount == 0)
                return;
            startedActionsCount++;
            if (context.interaction is SlowTapInteraction)
                StartCharging();
            StartedEvent?.Invoke();
        }

        void OnPerformed(InputAction.CallbackContext context) {
            Debug.Log($"OnPerformed");
            if (context.interaction is SlowTapInteraction) {
                StopCharging();
                PerformAttack();
                CurrentAttackIsStronk = true;
            } else {
                PerformAttack();
                CurrentAttackIsStronk = false;
            }
            startedActionsCount = 0;
        }

        void PerformAttack() {
            animator.SetTrigger($"Attack{attackType}");
        }

        void OnCanceled(InputAction.CallbackContext context) {
            if (context.interaction is SlowTapInteraction) {
                StopCharging();
                startedActionsCount = 0;
                animator.SetTrigger("CancelCharge");
            }
        }

        void StartCharging() {
            IsCharging = true;
            chargingVisuals.Play();
            animator.SetBool($"IsCharging{attackType}", true);
        }

        void StopCharging() {
            IsCharging = false;
            animator.SetBool($"IsCharging{attackType}", false);
            chargingVisuals.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
