using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class CameraSystem : BaseGameSystem {
        [SerializeField] Camera mainCameraPrefab;

        CinemachineVirtualCamera virtualCamera;

        public Camera MainCamera { get; private set; }


        public override void OnCreate() {
            MainCamera = Instantiate(mainCameraPrefab);
        }

        void HandlePlayerInstantiated(PlayerController player) {
            virtualCamera = player.Camera;
        }

        public override void Initialise() {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstantiatedEvent += HandlePlayerInstantiated;
        }

        public async UniTaskVoid FocusOnMe(Transform target, float duration, Action onCompleteCallback) {
            GameSystems.GetSystem<InputSystem>().DisableInput();
            var followBackup = virtualCamera.Follow;
            virtualCamera.Follow = target;
            await UniTask.Delay((int)(duration * 1000));
            virtualCamera.Follow = followBackup;
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
            onCompleteCallback?.Invoke();
        }
    }
}
