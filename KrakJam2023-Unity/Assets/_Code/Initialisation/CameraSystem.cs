using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class CameraSystem : BaseGameSystem {
        [SerializeField] Camera mainCameraPrefab;
        // [SerializeField] CrosshairController crosshairPrefab;

        CinemachineVirtualCamera virtualCamera;

        public Camera MainCamera { get; private set; }
        // public CrosshairController CrosshairInstance { get; private set; }


        public override void OnCreate() {
            MainCamera = Instantiate(mainCameraPrefab);
        }

        public override void Initialise() {
            // CrosshairInstance = Instantiate(crosshairPrefab, MainCamera.transform, false);
        }


        public void SetupCrosshairFollowTarget(Transform followTarget) {
            // CrosshairInstance.Setup(MainCamera, followTarget);
        }

        public async UniTaskVoid FocusOnMe(Transform target) {
            GameSystems.GetSystem<InputSystem>().DisableInput();
            var followBackup = virtualCamera.Follow;
            virtualCamera.Follow = target;
            await UniTask.Delay(2000);
            virtualCamera.Follow = followBackup;
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
        }
    }
}
