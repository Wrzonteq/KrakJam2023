using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PartTimeKamikaze.KrakJam2023 {
    public class ChargeableSkillWrapper {
        Action startedCallback;
        Action normalCallback;
        Action<float> strongCallback;
        ParticleSystem chargingVisuals;
        float chargingStartTime;
        PlayerController player;

        int startedActionsCount;


        public bool IsCharging { get; private set; }
        public bool CurrentAttackIsStronk { get; private set; }


        public ChargeableSkillWrapper(InputAction action, Action startedCallback, Action normalCallback, Action<float> strongCallback, ParticleSystem chargingVisuals) {
            player = GameSystems.GetSystem<GameplaySystem>().PlayerInstance;
            this.startedCallback = startedCallback;
            this.normalCallback = normalCallback;
            this.strongCallback = strongCallback;
            this.chargingVisuals = chargingVisuals;
            action.started += OnStarted;
            action.performed += OnPerformed;
            action.canceled += OnCanceled;
        }

        void OnStarted(InputAction.CallbackContext context) {
            if (player.IsInputLocked() && startedActionsCount == 0)
                return;
            startedActionsCount++;
            if (context.interaction is SlowTapInteraction)
                StartCharging();
            startedCallback?.Invoke();
        }

        void OnPerformed(InputAction.CallbackContext context) {
            if (context.interaction is SlowTapInteraction) {
                StopCharging();
                var chargeDuration = Time.time - chargingStartTime;
                strongCallback?.Invoke(chargeDuration);
                CurrentAttackIsStronk = true;
            } else {
                CurrentAttackIsStronk = false;
                normalCallback?.Invoke();
            }

            startedActionsCount = 0;
        }

        void OnCanceled(InputAction.CallbackContext context) {
            if (context.interaction is SlowTapInteraction) {
                StopCharging();
                startedActionsCount = 0;
            }
        }

        void StartCharging() {
            IsCharging = true;
            chargingVisuals.Play();
            chargingStartTime = Time.time;
        }

        void StopCharging() {
            IsCharging = false;
            chargingVisuals.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
