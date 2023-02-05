using PartTimeKamikaze.KrakJam2023.Utils;
using UnityEngine;
using UnityEngine.VFX;

namespace PartTimeKamikaze.KrakJam2023 {
    public class LightningVfx : MonoBehaviour {
        [SerializeField] VisualEffect vfx;

        public void Play() {
            vfx.gameObject.SetActive(true);
            vfx.Play();
        }

        public void Stop() {
            vfx.Stop();
        }

        public void SetLength(float length) {
            vfx.transform.SetLocalScaleZ(length);
        }

        public void SetWidth(float width) {
            vfx.transform.SetLocalScaleX(width);
        }
    }
}
