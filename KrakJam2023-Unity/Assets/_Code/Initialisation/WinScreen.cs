using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2023.UI {
    public class WinScreen : UiScreenBase {
        [SerializeField] Button returnButton;

        protected override float FadeInDuration => 0;
        protected override float FadeOutDuration => 0;


        protected override void OnInitialise() {
            returnButton.onClick.AddListener(HandleReturnButton);
        }

        void HandleReturnButton() {
            GameSystems.GetSystem<SceneLoadingSystem>().UnloadSceneAsync(Consts.ScenesNames.Gameplay).Forget();
            GameSystems.GetSystem<UISystem>().ShowScreen<MainMenuScreen>();
            Hide().Forget();
        }

        // protected override void OnShow() {
        //     GameSystems.GetSystem<InputSystem>().Bindings.Interface.Cancel.performed += HandleCancelInput;
        // }
        //
        // void HandleCancelInput(InputAction.CallbackContext _) {
        //     Hide().Forget();
        // }
        //
        // protected override void OnHide() {
        //     GameSystems.GetSystem<InputSystem>().Bindings.Interface.Cancel.performed -= HandleCancelInput;
        // }
    }
}
