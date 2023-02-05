using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2023.UI {
    public class IntroScreen : UiScreenBase, IPointerClickHandler {
        [SerializeField] CanvasGroup[] slides;
        [SerializeField] TextMeshProUGUI skipLabel;
        [SerializeField] float slideFadeInDuration = 1f;
        [SerializeField] float slideDuration = 3f;

        bool awaitingSecondInput;
        bool skipRequested;


        protected override void OnShow() {
            GameSystems.GetSystem<InputSystem>().Bindings.Interface.Continue.performed += HandleContinue;
            awaitingSecondInput = false;
            skipLabel.alpha = 0;
            foreach (var slide in slides)
                slide.alpha = 0;
        }

        void HandleContinue(InputAction.CallbackContext _) {
            Debug.Log($"HandleContinue");
            if (awaitingSecondInput) {
                Skip();
            } else {
                skipLabel.DOKill();
                skipLabel.DOFade(1, 0);
                skipLabel.DOFade(0, 1).SetDelay(1).OnComplete(() => awaitingSecondInput = false);
                awaitingSecondInput = true;
            }
        }

        void Skip() {
            skipRequested = true;
        }

        public async UniTask StartSlideShow() {
            foreach (var slide in slides) {
                Debug.Log($"Display slide {slide.name}");
                slide.DOFade(1, slideFadeInDuration);
                var nextSlideTime = Time.time + slideDuration;
                while (nextSlideTime > Time.time) {
                    if (skipRequested) {
                        skipRequested = false;
                        break;
                    }
                    await UniTask.DelayFrame(1);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            HandleContinue(default);
        }
    }
}
