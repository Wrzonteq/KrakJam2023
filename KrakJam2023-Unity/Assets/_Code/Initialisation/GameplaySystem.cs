using System;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2023.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2023 {
    public class GameplaySystem : BaseGameSystem {
        public event Action<PlayerController> PlayerInstantiatedEvent;
        public event Action ReturnedToMainMenu;

        [SerializeField] PlayerController playerPrefab;


        public PlayerController PlayerInstance { get; private set; }

        bool isInGameplay;
        public bool IsInGameplay { get =>  isInGameplay;
            private set {
                isInGameplay = value;
                if (value)
                    OnGameplayStart();
                else
                    OnGameplayEnd();
            }
        }

        public override void OnCreate() {
        }

        public override void Initialise() { }

        void OnGameplayStart() {
            GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.OpenPauseMenu.performed += OpenPauseScreen;
        }

        void OnGameplayEnd() {
            GameSystems.GetSystem<InputSystem>().Bindings.Gameplay.OpenPauseMenu.performed -= OpenPauseScreen;
        }

        public void StartNewGame() {
            GameSystems.GetSystem<GameStateSystem>().ResetGameState();
            LoadSavedGame(GameSystems.GetSystem<GameStateSystem>().runtimeGameState).Forget();
        }

        public async UniTaskVoid LoadSavedGame(GameStateDataAsset gameState) {
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
            await LoadGameplaySceneAndShowProgress();
            LoadGame(gameState);
        }

        async UniTask LoadGameplaySceneAndShowProgress() {
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Hide(true).Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.LoadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            GameSystems.GetSystem<UISystem>().ShowScreen<HUDScreen>().Forget();
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();
        }

        void LoadGame(GameStateDataAsset gameState) {
            GameSystems.GetSystem<GameStateSystem>().LoadCurrentStateToProperties();
            SpawnPlayer();
            // GameSystems.GetSystem<CameraSystem>().SetupCrosshairFollowTarget(PlayerInstance.CrosshairFollowTarget);
            
            IsInGameplay = true;
        }

        void SpawnPlayer() {
            GameObject[] spawne_points = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
            PlayerInstance = Instantiate(playerPrefab, spawne_points[0].transform.position, Quaternion.identity);
            PlayerInstance.Initialise();
            PlayerInstantiatedEvent?.Invoke(PlayerInstance);
        }

        void OpenPauseScreen(InputAction.CallbackContext _) {
            GameSystems.GetSystem<UISystem>().GetScreen<PauseMenuScreen>().Show().Forget();
        }

        void DisplayProgress(float progress) {
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().SetProgress(progress);
        }

        public async UniTaskVoid ReturnToMenu() {
            IsInGameplay = false;
            GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Show(true).Forget();
            GameSystems.GetSystem<UISystem>().HideScreen<HUDScreen>().Forget();
            var sceneLoadingSystem = GameSystems.GetSystem<SceneLoadingSystem>();
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue += DisplayProgress;
            await sceneLoadingSystem.UnloadSceneAsync(Consts.ScenesNames.Gameplay);
            sceneLoadingSystem.SceneLoadingProgress.ChangedValue -= DisplayProgress;
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Show(true).Forget();
            await GameSystems.GetSystem<UISystem>().GetScreen<LoadingScreen>().Hide();
            ReturnedToMainMenu?.Invoke();
        }
    }
}
