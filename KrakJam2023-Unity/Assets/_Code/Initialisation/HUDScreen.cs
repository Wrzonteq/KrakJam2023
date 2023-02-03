using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023.UI {
    public class HUDScreen : UiScreenBase {
        protected override float FadeInDuration => 0f;
        protected override float FadeOutDuration => 0f;

        protected override void OnInitialise() {
            var gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            gameStateSystem.Insanity.ChangedValue += SetProgress;
            gameStateSystem.Stage.ChangedValue += HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
        }

        void SetProgress(int percentage) {
            // insanityDisplay.SetFill(percentage / 100f);
        }
    }
}
