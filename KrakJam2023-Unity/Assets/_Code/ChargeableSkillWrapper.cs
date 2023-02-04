using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PartTimeKamikaze.KrakJam2023 {
    public class ChargeableSkillWrapper {
        InputSystem inputSystem;
        Action normalCallback;
        Action<float> strongCallback;
        ParticleSystem chargingVisuals;
        float chargingStartTime;

        public bool IsCharging { get; private set; }


        public ChargeableSkillWrapper(InputAction action, Action normalCallback, Action<float> strongCallback, ParticleSystem chargingVisuals) {
            inputSystem = GameSystems.GetSystem<InputSystem>();
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
