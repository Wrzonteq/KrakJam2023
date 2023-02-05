using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2023.UI {
    public class LoadingScreen : UiScreenBase {
        [SerializeField] Image loadingBarFill;

        int lastProgress = -1;


        public void SetProgress(float progress) {
            var percentage = Mathf.CeilToInt(progress * 100);
            if (percentage == lastProgress)
                return; // to optimise gui redraws
            lastProgress = percentage;
            loadingBarFill.fillAmount = progress;
        }

        protected override void OnHide() {
            
        }
    }
}
