using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PartTimeKamikaze.KrakJam2023 {
    public class ChargeableSkillWrapper {
        InputSystem inputSystem;
        InputAction action;
        Action normalCallback;
        Action<float> strongCallback;
        GameObject chargingVisuals;
        bool isCharging;
        float chargingStartTime;


        public ChargeableSkillWrapper(InputAction action, Action normalCallback, Action<float> strongCallback, GameObject chargingVisuals) {
            inputSystem = GameSystems.GetSystem<InputSystem>();
            this.action = action;
            this.normalCallback = normalCallback;
            this.strongCallback = strongCallback;
            this.chargingVisuals = chargingVisuals;
            action.started += OnStarted;
            action.performed += OnPerformed;
            action.canceled += OnCanceled;
        }

        void OnStarted(InputAction.CallbackContext context) {
            if (!inputSystem.PlayerInputEnabled)
                return;
            if (context.interaction is SlowTapInteraction)
                StartCharging();
            else {

            }
        }

        void OnPerformed(InputAction.CallbackContext context) {
            if (!inputSystem.PlayerInputEnabled)
                return;
            if (context.interaction is SlowTapInteraction) {
                StopCharging();
                var chargeDuration = Time.time - chargingStartTime;
                strongCallback?.Invoke(chargeDuration);
            } else {
                normalCallback?.Invoke();
            }
        }

        void OnCanceled(InputAction.CallbackContext context) {
            if (!inputSystem.PlayerInputEnabled)
                return;
            if (context.interaction is SlowTapInteraction)
                StopCharging();
        }

        void StartCharging() {
            isCharging = true;
            chargingVisuals.SetActive(true);
            chargingStartTime = Time.time;
        }

        void StopCharging() {
            isCharging = false;
            chargingVisuals.SetActive(false);
        }
    }
}
